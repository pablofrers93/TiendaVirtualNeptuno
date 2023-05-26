using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;

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
            //var lista = _servicio.GetPaises();
            var lista = new List<Pais>();
            return View(lista);
        }
    }
}