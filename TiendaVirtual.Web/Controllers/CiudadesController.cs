using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.ViewModels.Ciudad;

namespace TiendaVirtual.Web.Controllers
{
    public class CiudadesController : Controller
    {
        // GET: Ciudades
        private readonly IServiciosCiudades _servicio;
        public CiudadesController(IServiciosCiudades servicio)
        {
            _servicio = servicio;
        }
        public ActionResult Index()
        {
            var lista = _servicio.GetCiudades();
            var listaVm = GetCiudadesListVm(lista);
            return View(listaVm);
        }
        private List<CiudadListVm> GetCiudadesListVm(List<CiudadListDto>lista)
        {
            var listaVm = new List<CiudadListVm>();
            foreach (var item in lista)
            {
                var ciudadVm = new CiudadListVm()
                {
                    CiudadId = item.CiudadId,
                    NombreCiudad = item.NombreCiudad,
                    NombrePais = item.NombrePais
                };
                listaVm.Add(ciudadVm);
            }
            return listaVm;
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ciudad ciudad)
        {
            if (_servicio.Existe(ciudad))
            {
                ModelState.AddModelError(string.Empty, "Ciudad existente");
                return View();
            }
            _servicio.Guardar(ciudad);
            return RedirectToAction("Index");
        }
    }
}