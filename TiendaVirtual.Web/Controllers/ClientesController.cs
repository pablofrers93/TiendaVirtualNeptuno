using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.App_Start;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Cliente;

namespace TiendaVirtual.Web.Controllers
{
    public class ClientesController : Controller
    {
        // GET: Clientes
        private readonly IServiciosClientes _servicios;
        private readonly IServiciosPaises _serviciosPaises;
        private readonly IServiciosCiudades _serviciosCiudades;
        private readonly IMapper _mapper;

        public ClientesController(IServiciosClientes servicios, IServiciosPaises serviciosPaises, IServiciosCiudades serviciosCiudades)
        {
            _servicios = servicios;
            _serviciosPaises = serviciosPaises;
            _serviciosCiudades = serviciosCiudades;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            var lista = _servicios.GetClientes();
            var listaVm = _mapper.Map<List<ClienteListVm>>(lista);
            return View(listaVm);
        }
        public ActionResult Create()
        {
            var clienteVm = new ClienteEditVm()
            {
                Paises = _serviciosPaises.GetPaisesDropDownList(),
                Ciudades = new List<SelectListItem>()
            };
            return View(clienteVm);
        }
        public JsonResult GetCities(int paisId)
        {
            var lista = _serviciosCiudades.GetCiudades(paisId);
            var ciudadesVm = _mapper.Map<List<CiudadListVm>>(lista);
            return Json(ciudadesVm);
        }
    }
}