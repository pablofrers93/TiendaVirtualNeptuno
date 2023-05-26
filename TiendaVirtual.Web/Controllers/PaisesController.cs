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
    }
}