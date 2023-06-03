using AutoMapper;
using Neptuno2022EF.Servicios.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.App_Start;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Pais;

namespace TiendaVirtual.Web.Controllers
{
    public class CiudadesController : Controller
    {
        // GET: Ciudades
        private readonly IServiciosCiudades _servicio;
        private readonly IServiciosPaises _servicioPaises;
        private readonly IMapper _mapper;
        public CiudadesController(IServiciosCiudades servicio, IServiciosPaises servicioPaises)
        {
            _servicio = servicio;
            _servicioPaises = servicioPaises;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            var lista = _servicio.GetCiudades();
            var listaVm = _mapper.Map<List<CiudadListVm>>(lista);
            return View(listaVm);
        }

        public ActionResult Create()
        {
            var listaPaises = _servicioPaises.GetPaises();
            var dropDownPaises = listaPaises.Select(p => new SelectListItem()
            {
                Text = p.NombrePais,
                Value = p.PaisId.ToString()
            }).ToList();

            var ciudadVm = new CiudadEditVm()
            {
                Paises = dropDownPaises
            };
            return View(ciudadVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CiudadEditVm ciudadVm)
        {
            if (!ModelState.IsValid)
            {
                var listaPaises = _servicioPaises.GetPaises();
                var dropDownPaises = listaPaises.Select(p => new SelectListItem()
                {
                    Text = p.NombrePais,
                    Value = p.PaisId.ToString()
                }).ToList();
                
                ciudadVm.Paises = dropDownPaises; 
                return View(ciudadVm);
            }
            var ciudad = _mapper.Map<Ciudad>(ciudadVm);
            if (_servicio.Existe(ciudad))
            {
                var listaPaises = _servicioPaises.GetPaises();
                var dropDownPaises = listaPaises.Select(p => new SelectListItem()
                {
                    Text = p.NombrePais,
                    Value = p.PaisId.ToString()
                }).ToList();

                ciudadVm.Paises = dropDownPaises;
                ModelState.AddModelError(string.Empty, "Ciudad existente");
                return View(ciudadVm);
            }
            _servicio.Guardar(ciudad);
            TempData["Msg"] = "Registro guardado satisfactoriamente";
            return RedirectToAction("Index");
        }
    
        public ActionResult Delete(int? id)
        {
            if (id== null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ciudad = _servicio.GetCiudadPorId(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound("Código de ciudad inexistente");
            }
            var ciudadVm =  _mapper.Map<CiudadListVm>(ciudad);
            return View(ciudadVm);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var ciudad = _servicio.GetCiudadPorId(id);
            if (_servicio.EstaRelacionada(ciudad))
            {
                var ciudadVm = _mapper.Map<CiudadListVm>(ciudad);
                ModelState.AddModelError(string.Empty, "Ciudad relacionada... Baja denegada");
                return View(ciudadVm);
            }
            _servicio.Borrar(id);
            TempData["Msg"] = "Registro borrado satisfactoriamente";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ciudad = _servicio.GetCiudadPorId(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound("Codigo de ciudad inexistente");
            }
            var ciudadVm = _mapper.Map<CiudadEditVm>(ciudad);
            var listaPaises = _servicioPaises.GetPaises();
            ViewBag.DropDownPaises = new SelectList(listaPaises, "PaisId", "NombrePais");
            return View(ciudadVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CiudadEditVm ciudadVm)
        {
            if (!ModelState.IsValid)
            {
                return View(ciudadVm);
            }
            var ciudad = _mapper.Map<Ciudad>(ciudadVm);
            var listaPaises = _servicioPaises.GetPaises();
            ViewBag.DropDownPaises = new SelectList(listaPaises, "PaisId", "NombrePais");

            if (_servicio.Existe(ciudad))
            {
                ModelState.AddModelError(string.Empty, "Ciudad Existente");
                return View(ciudadVm);
            }
            _servicio.Guardar(ciudad);
            TempData["Msg"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }
        //private List<CiudadListVm> GetCiudadesListVm(List<CiudadListDto>lista)
        //{
        //    var listaVm = new List<CiudadListVm>();
        //    foreach (var item in lista)
        //    {
        //        var ciudadVm = new CiudadListVm()
        //        {
        //            CiudadId = item.CiudadId,
        //            NombreCiudad = item.NombreCiudad,
        //            NombrePais = item.NombrePais
        //        };
        //        listaVm.Add(ciudadVm);
        //    }
        //    return listaVm;
        //}
    }
}