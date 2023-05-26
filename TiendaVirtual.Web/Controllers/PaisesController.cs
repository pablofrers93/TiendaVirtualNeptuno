using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.ViewModels.Pais;

namespace TiendaVirtual.Web.Controllers
{
    public class PaisesController : Controller
    {
        // GET: Paises
        private readonly IServiciosPaises _servicio;
        public PaisesController(IServiciosPaises servicios)
        {
            _servicio = servicios;
        }
        public ActionResult Index()
        {
            var lista = _servicio.GetPaises();
            var listaVm = GetListaPaisesListVm(lista);
            return View(listaVm);
        }
        private List<PaisListVm> GetListaPaisesListVm(List<Pais> lista)
        {
            var listaVm = new List<PaisListVm>();
            foreach (var item in lista)
            {
                var paisVm = new PaisListVm()
                {
                    PaisId = item.PaisId,
                    NombrePais = item.NombrePais
                };
                listaVm.Add(paisVm);
            }
            return listaVm;
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PaisEditVm paisVm)
        {
            if (ModelState.IsValid)
            {
                var pais = GetPaisFromPaisEditVm(paisVm);
                if (_servicio.Existe(pais))
                {
                    ModelState.AddModelError(string.Empty, "País existente");
                    return View(paisVm);
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
                return View(paisVm);
            }
        }
        private Pais GetPaisFromPaisEditVm(PaisEditVm paisEditVm)
        {
            return new Pais()
            {
                PaisId = paisEditVm.PaisId,
                NombrePais = paisEditVm.NombrePais
            };
        }
    }
}