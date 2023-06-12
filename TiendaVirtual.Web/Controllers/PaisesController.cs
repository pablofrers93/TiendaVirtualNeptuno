using AutoMapper;
using Neptuno2022EF.Servicios.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.App_Start;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Pais;

namespace TiendaVirtual.Web.Controllers
{
    public class PaisesController : Controller
    {
        // GET: Paises
        private readonly IServiciosPaises _servicio;
        private readonly IServiciosCiudades _servicioCiudades;
        private readonly IMapper _mapper;
        public PaisesController(IServiciosPaises servicios, IServiciosCiudades servicioCiudades)
        {
            _servicio = servicios;
            _mapper = AutoMapperConfig.Mapper;
            _servicioCiudades = servicioCiudades;
        }
        public ActionResult Index()
        {
            var lista = _servicio.GetPaises();
            //var listaVm = GetListaPaisesListVm(lista);
            var listaVm = _mapper.Map<List<PaisListVm>>(lista);
            listaVm.ForEach(p => p.CantidadCiudades = _servicioCiudades.GetCantidad(c => c.PaisId == p.PaisId));
            
            return View(listaVm);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(PaisEditVm paisVm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var pais = _mapper.Map<Pais>(paisVm);
        //        if (_servicio.Existe(pais))
        //        {
        //            ModelState.AddModelError(string.Empty, "País existente");
        //            return View(paisVm);
        //        }
        //        else
        //        {
        //            _servicio.Guardar(pais);
        //            TempData["Msg"] = "Registro guardado satisfactoriamente";
        //            return RedirectToAction("Index");
        //        } 
        //    }
        //    else
        //    {
        //        return View(paisVm);
        //    }
        //}
        [HttpPost]
        public ActionResult Create(string paisSeleccionado)
        {
            if (ModelState.IsValid)
            {
                var pais = new Pais();
                pais.NombrePais = paisSeleccionado;
                if (_servicio.Existe(pais))
                {
                    ModelState.AddModelError(string.Empty, "País existente");
                    return View(); 
                }
                else if (pais.NombrePais == "")
                {
                    ModelState.AddModelError(string.Empty, "Debe seleccionar un país");
                    return View();
                }
                else
                {
                    _servicio.Guardar(pais);
                    TempData["Msg"] = "Registro guardado satisfactoriamente";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(paisSeleccionado);
            }
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var pais = _servicio.GetPaisPorId(id.Value);
            if (pais == null)
            {
                return HttpNotFound("Codigo de país inexistente");
            }
            var paisVm = _mapper.Map<PaisListVm>(pais);
            return View(paisVm);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var pais = _servicio.GetPaisPorId(id);
            if (_servicio.EstaRelacionado(pais))
            {
                var paisVm = _mapper.Map<PaisListVm>(pais);
                ModelState.AddModelError(string.Empty, "País relacionado... Baja denegada");
                return View(paisVm);
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
            var pais = _servicio.GetPaisPorId(id.Value);
            if (pais == null)
            {
                return HttpNotFound("Codigo de país inexistente");
            }
            var paisVm = _mapper.Map<PaisEditVm>(pais);
            return View(paisVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaisEditVm paisVm) 
        {
            if (!ModelState.IsValid)
            {
                return View(paisVm);
            }
            var pais = _mapper.Map<Pais>(paisVm);
            if (_servicio.Existe(pais))
            {
                ModelState.AddModelError(string.Empty, "Pais Existente");
                return View(paisVm);
            }
            _servicio.Guardar(pais);
            TempData["Msg"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var pais = _servicio.GetPaisPorId(id.Value);
            if (pais==null)
            {
                return HttpNotFound("Codigo de pais inexistente");
            }
            var paisVm = _mapper.Map<PaisListVm>(pais);
            paisVm.CantidadCiudades = _servicioCiudades.GetCantidad(c => c.PaisId == paisVm.PaisId);
            var paisDetailVm = new PaisDetailVm()
            {
                Pais = paisVm,
                Ciudades = _mapper.Map<List<CiudadListVm>>(_servicioCiudades.GetCiudades(pais.PaisId))
            };
            return View(paisDetailVm);
        }

        public ActionResult AddCity(int? paisId)
        {
            if (paisId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var pais = _servicio.GetPaisPorId(paisId.Value);
            if (pais == null)
            {
                return HttpNotFound("Codigo de pais inexistente");
            }
            var ciudadVm = new CiudadEditVm();
            CargarPaises(ciudadVm);
            return View(ciudadVm); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCity(CiudadEditVm ciudadVm)
        {
            if (!ModelState.IsValid)
            {
                var listaPaises = _servicio.GetPaises();
                CargarPaises(ciudadVm);
                return View (ciudadVm);
            }
            try
            {
                var ciudad = _mapper.Map<Ciudad>(ciudadVm);
                if (!_servicioCiudades.Existe(ciudad))
                {
                    _servicioCiudades.Guardar(ciudad);
                    return RedirectToAction($"Details/{ciudad.PaisId}");
                }
                else
                {
                    CargarPaises(ciudadVm);
                    ModelState.AddModelError(string.Empty, "Ciudad existente");
                    return View (ciudadVm);
                }
            }
            catch (Exception)
            {
                CargarPaises(ciudadVm);
                ModelState.AddModelError(string.Empty, "Error al intentar agregar una ciudad");
                return View(ciudadVm);
            }
        }
        public ActionResult DeleteCity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ciudad = _servicioCiudades.GetCiudadPorId(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound("Codigo de ciudad inexistente");
            }
            var ciudadVm = _mapper.Map<CiudadEditVm>(ciudad);
            CargarPaises(ciudadVm);
            return View(ciudadVm);
        }
        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCityConfirm(int id)
        {
            var ciudad = _servicioCiudades.GetCiudadPorId(id);
            var ciudadVm = _mapper.Map<CiudadEditVm>(ciudad);
            try
            {
                if (!_servicioCiudades.EstaRelacionada(ciudad))
                {
                    _servicioCiudades.Borrar(id);
                    return RedirectToAction($"Details/{ciudad.PaisId}");
                }
                else
                {
                    CargarPaises(ciudadVm);
                    ModelState.AddModelError(string.Empty, "Ciudad relacionada, baja denegada");
                    return View(ciudadVm);
                }
            }
            catch (Exception)
            {
                CargarPaises(ciudadVm);
                ModelState.AddModelError(string.Empty, "Error al intentar borrar la ciudad");
                return View(ciudadVm);
            }
        }

        public ActionResult EditCity (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var ciudad = _servicioCiudades.GetCiudadPorId(id.Value);
            if (ciudad == null)
            {
                return HttpNotFound("Codigo de ciudad inexistente");
            }
            var ciudadVm = _mapper.Map<CiudadEditVm>(ciudad);
            ciudadVm.PaisAnteriorId = ciudadVm.PaisId;
            CargarPaises(ciudadVm);
            return View(ciudadVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCity(CiudadEditVm ciudadVm)
        {
            if (!ModelState.IsValid)
            {
                CargarPaises(ciudadVm);
                return View(ciudadVm);
            }
            try
            {
                var ciudad = _mapper.Map<Ciudad>(ciudadVm);
                if (!_servicioCiudades.Existe(ciudad))
                {
                    _servicioCiudades.Guardar(ciudad);
                    return RedirectToAction($"Details/{ciudadVm.PaisAnteriorId}");
                }
                else
                {
                    CargarPaises(ciudadVm);
                    ModelState.AddModelError(string.Empty, "Ciudad existente");
                    return View(ciudadVm);
                }

            }
            catch (Exception)
            {
                CargarPaises(ciudadVm);
                ModelState.AddModelError(string.Empty, "Error al agregar la ciudad");
                return View(ciudadVm);
            }
        }
        private void CargarPaises(CiudadEditVm ciudadVm)
        {
            var listaPaises = _servicio.GetPaises();
            ciudadVm.Paises = listaPaises.Select(p => new SelectListItem()
            {
                Text = p.NombrePais,
                Value = p.PaisId.ToString()
            }).ToList();
        }

        //private PaisEditVm GetPaisEditVmFromPais(Pais pais)
        //{
        //    return new PaisEditVm()
        //    {
        //        PaisId = pais.PaisId,
        //        NombrePais = pais.NombrePais,
        //        RowVersion = pais.RowVersion
        //    };
        //}

        //private Pais GetPaisFromPaisEditVm(PaisEditVm paisEditVm)
        //{
        //    return new Pais()
        //    {
        //        PaisId = paisEditVm.PaisId,
        //        NombrePais = paisEditVm.NombrePais,
        //        RowVersion = paisEditVm.RowVersion
        //    };
        //}
        //private PaisListVm GetPaisListVm(Pais pais)
        //{
        //    return new PaisListVm()
        //    {
        //        PaisId = pais.PaisId,
        //        NombrePais = pais.NombrePais
        //    };
        //}
        //private List<PaisListVm> GetListaPaisesListVm(List<Pais> lista)
        //{
        //    var listaVm = new List<PaisListVm>();
        //    foreach (var item in lista)
        //    {
        //        var paisVm = GetPaisListVm(item);
        //        listaVm.Add(paisVm);
        //    }
        //    return listaVm;
        //}
    }
}