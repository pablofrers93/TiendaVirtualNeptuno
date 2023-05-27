using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;
using TiendaVirtual.Web.ViewModels.Categoria;

namespace TiendaVirtual.Web.Controllers
{
    public class CategoriasController : Controller
    {
        // GET: Categorias
        private readonly IServiciosCategorias _servicio;
        public CategoriasController(IServiciosCategorias servicio)
        {
            _servicio = servicio;
        }
        public ActionResult Index()
        {
            var lista = _servicio.GetCategorias();
            var listaVm = GetListaCategoriasListVm(lista);
            return View(listaVm);
        }
        private List<CategoriaListVm>GetListaCategoriasListVm(List<Categoria>lista)
        {
            var listaVm = new List<CategoriaListVm>();
            foreach (var item in lista)
            {
                var categoriaVm = GetCategoriaListVm(item);
                listaVm.Add(categoriaVm);
            }
            return listaVm;
        }
        private CategoriaListVm GetCategoriaListVm(Categoria item)
        {
            return new CategoriaListVm()
            {
                CategoriaId = item.CategoriaId,
                NombreCategoria = item.NombreCategoria,
                Descripcion = item.Descripcion,
                RowVersion = item.RowVersion
            };
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoriaEditVm categoriaVm)
        {
            if (ModelState.IsValid)
            {
                var categoria = GetCategoriaFromCategoriaEditVm(categoriaVm);
                if (_servicio.Existe(categoria))
                {
                    ModelState.AddModelError(string.Empty, "Categoría existente");
                    return View(categoriaVm);
                }
                else
                {
                    _servicio.Guardar(categoria);
                    TempData["Msg"] = "Registro guardado satisfactoriamente";
                    return RedirectToAction("Index");
                }                  
            } 
            else
            {
                return View(categoriaVm);
            }
        }
        private Categoria GetCategoriaFromCategoriaEditVm(CategoriaEditVm categoriaEditVm)
        {
            return new Categoria()
            {
                CategoriaId = categoriaEditVm.CategoriaId,
                NombreCategoria = categoriaEditVm.NombreCategoria,
                Descripcion = categoriaEditVm.Descripcion,
                RowVersion = categoriaEditVm.RowVersion
            };
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var categoria = _servicio.GetCategoriaPorId(id.Value);
            if (categoria == null)
            {
                return HttpNotFound("Código de categoría inexistente");
            }
            var categoriaVm = GetCategoriaListVm(categoria);
            return View(categoriaVm);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm (int id)
        {
            var categoria = _servicio.GetCategoriaPorId(id);
            if (_servicio.EstaRelacionado(categoria))
            {
                var categoriaVm = GetCategoriaListVm(categoria);
                ModelState.AddModelError(string.Empty, "Categoría relacionada...Baja denegada");
                return View(categoriaVm);
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
            var categoria = _servicio.GetCategoriaPorId(id.Value);
            if (categoria == null)
            {
                return HttpNotFound("Código de categoría inexistente");
            }
            var categoriaVm = GetCategoriaEditVmFromCategoria(categoria);
            return View(categoriaVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (CategoriaEditVm categoriaVm)
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaVm);   
            }
            var categoria = GetCategoriaFromCategoriaEditVm(categoriaVm);
            if (_servicio.Existe(categoria))
            {
                ModelState.AddModelError(string.Empty, "Categoría existente...");
                return View(categoriaVm);
            }
            _servicio.Guardar(categoria);
            TempData["Msg"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }
        private CategoriaEditVm GetCategoriaEditVmFromCategoria(Categoria categoria)
        {
            return new CategoriaEditVm()
            {
                CategoriaId = categoria.CategoriaId,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion,
                RowVersion = categoria.RowVersion
            };
        }
    }
}