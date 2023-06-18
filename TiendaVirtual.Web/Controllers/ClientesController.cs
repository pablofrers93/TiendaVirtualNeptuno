using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades.Entidades;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (ClienteEditVm clienteVm)
        {
            if (!ModelState.IsValid)
            {
                clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
                clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(clienteVm.PaisId);
                return View(clienteVm);
            }
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteVm);
                if (!_servicios.Existe(cliente))
                {
                    _servicios.Guardar(cliente);
                    TempData["Msg"] = "Registro agregado satisfactoriamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cliente existente");
                    clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
                    clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(clienteVm.PaisId);
                    return View(clienteVm);   
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al agregar el cliente");
                clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
                clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(clienteVm.PaisId);
                return View(clienteVm);
            }
        }
        public JsonResult GetCities(int paisId)
        {
            var lista = _serviciosCiudades.GetCiudades(paisId);
            var ciudadesVm = _mapper.Map<List<CiudadListVm>>(lista);
            return Json(ciudadesVm);
        }
        public ActionResult Edit (int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var cliente = _servicios.GetClientePorId(id.Value);
            if (cliente == null)
            {
                return HttpNotFound("Codigo de cliente inexistente");
            }
            var clienteVm = _mapper.Map<ClienteEditVm>(cliente);
            clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
            clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(cliente.PaisId);
            return View(clienteVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (ClienteEditVm clienteVm)
        {
            if (!ModelState.IsValid)
            {
                clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
                clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(clienteVm.PaisId);
                return View(clienteVm);
            }
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteVm);
                if (!_servicios.Existe(cliente))
                {
                    _servicios.Guardar(cliente);
                    TempData["Msg"] = "Registro editado satisfactoriamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cliente existente");
                    clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
                    clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(clienteVm.PaisId);
                    return View(clienteVm);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al editar el cliente");
                clienteVm.Paises = _serviciosPaises.GetPaisesDropDownList();
                clienteVm.Ciudades = _serviciosCiudades.GetCiudadesDropDownList(clienteVm.PaisId);
                return View(clienteVm);
            }
        }
        public ActionResult Delete (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var cliente = _servicios.GetClientePorId(id.Value);
            if (cliente == null)
            {
                return HttpNotFound("Codigo de cliente inexistente");
            }
            var clienteVm = _mapper.Map<ClienteListVm>(cliente);
            return View(clienteVm);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm (int id) 
        {
            var cliente = _servicios.GetClientePorId(id);
            var clienteVm = _mapper.Map<ClienteListVm>(cliente);
            try
            {
                if (!_servicios.EstaRelacionado(cliente))
                {
                    _servicios.Borrar(id);
                    TempData["Msg"] = "Cliente borrado satisfactoriamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registro relacionado, baja denegada");
                    return View(clienteVm); 
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al eliminar el cliente");
                return View(clienteVm);
            }
        }
    }
}