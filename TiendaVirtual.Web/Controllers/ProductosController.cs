using AutoMapper;
using Neptuno2022EF.Servicios.Servicios;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Dtos.Producto;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Utilidades;
using TiendaVirtual.Web.App_Start;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Cliente;
using TiendaVirtual.Web.ViewModels.Producto;

namespace TiendaVirtual.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IServiciosProductos _servicios;
        private readonly IServiciosProveedores _serviciosProveedores;
        private readonly IServiciosCategorias _serviciosCategorias;
        private readonly IMapper _mapper;
        public ProductosController(IServiciosProductos servicios,
            IServiciosProveedores serviciosProveedores,
            IServiciosCategorias serviciosCategorias)
        {
            _servicios = servicios;
            _serviciosProveedores = serviciosProveedores;
            _serviciosCategorias = serviciosCategorias;
            _mapper = AutoMapperConfig.Mapper;
        }
        // GET: Productos
        public ActionResult Index()
        {
            var lista = _servicios.GetProductos();
            var listaVm = _mapper.Map<List<ProductoListVm>>(lista);
            return View(listaVm);
        }

        public ActionResult Create()
        {
            var productoVm = new ProductoEditVm
            {
                Proveedores = _serviciosProveedores.GetProveedoresDropDownList(),
                Categorias = _serviciosCategorias.GetCategoriasDropDownList()

            };
            return View(productoVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductoEditVm productoVm)
        {
            if (!ModelState.IsValid)
            {
                productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
                productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();
                return View(productoVm);
            }
            try
            {
                var producto = _mapper.Map<Producto>(productoVm);
                if (!_servicios.Existe(producto))
                {
                    if (productoVm.imagenFile != null)
                    {
                        string extension = Path.GetExtension(productoVm.imagenFile.FileName);
                        string filename = Guid.NewGuid().ToString();

                        var file = $"{filename}{extension}";
                        var response = FileHelper.UploadPhoto(productoVm.imagenFile, WC.ProductosImagenesFolder, file);
                        producto.Imagen = file;
                    }

                    _servicios.Guardar(producto);
                    TempData["Msg"] = "Registro agregado satisfactoriamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Producto existente!!!");
                    productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
                    productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();

                    return View(productoVm);
                }
            }
            catch (System.Exception)
            {

                ModelState.AddModelError(string.Empty, "Error al intentar agregar un Producto");
                productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
                productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();

                return View(productoVm);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var producto = _servicios.GetProductoPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound("Cód. de proveedor inexistente!!!");
            }
            var productoVm = _mapper.Map<ProductoEditVm>(producto);
            productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
            productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();
            return View(productoVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductoEditVm productoVm)
        {
            if (!ModelState.IsValid)
            {
                productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
                productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();
                return View(productoVm);

            }
            try
            {
                var producto = _mapper.Map<Producto>(productoVm);
                if (!_servicios.Existe(producto))
                {
                    _servicios.Guardar(producto);
                    TempData["Msg"] = "Registro editado satisfactoriamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Producto existente!!!");
                    productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
                    productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();
                    return View(productoVm);

                }
            }
            catch (System.Exception)
            {
                ModelState.AddModelError(string.Empty, "Producto existente!!!");
                productoVm.Proveedores = _serviciosProveedores.GetProveedoresDropDownList();
                productoVm.Categorias = _serviciosCategorias.GetCategoriasDropDownList();
                return View(productoVm);
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var producto = _servicios.GetProductoPorId(id.Value);
            if (producto == null)
            {
                return HttpNotFound("Cód. proveedor inexistente!!!");
            }
            var productoVm = _mapper.Map<ProductoListVm>(producto);
            return View(productoVm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var producto = _servicios.GetProductoPorId(id);
            var productoVm = _mapper.Map<ProductoListVm>(producto);
            try
            {
                if (!_servicios.EstaRelacionado(producto))
                {
                    _servicios.Borrar(id);
                    TempData["Msg"] = "Producto borrado satisfactoriamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registro relacionado... Baja denegada");
                    return View(productoVm);
                }
            }
            catch (System.Exception)
            {

                ModelState.AddModelError(string.Empty, "Error al intengar borrar un proveedor");
                return View(productoVm);

            }
        }
        //TODO: Hacer detalle de producto
    }
}