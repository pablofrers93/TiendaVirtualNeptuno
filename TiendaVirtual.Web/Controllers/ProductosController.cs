using AutoMapper;
using Neptuno2022EF.Servicios.Servicios;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Dtos.Producto;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.App_Start;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Cliente;
using TiendaVirtual.Web.ViewModels.Producto;

namespace TiendaVirtual.Web.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        private readonly IServiciosProductos _servicio;
        private readonly IMapper _mapper;
        public ProductosController(IServiciosProductos servicio)
        {
            _servicio = servicio;
            _mapper = AutoMapperConfig.Mapper;
        }
        public ActionResult Index()
        {
            var lista = _servicio.GetProductos();
            var listaVm = _mapper.Map<List<ProductoListVm>>(lista);
            return View(listaVm);
        }
    }
}