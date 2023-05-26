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
                var categoriaVm = new CategoriaListVm()
                {
                    CategoriaId = item.CategoriaId,
                    NombreCategoria = item.NombreCategoria,
                    Descripcion = item.Descripcion,
                    RowVersion = item.RowVersion
                };
                listaVm.Add(categoriaVm);
            }
            return listaVm;
        }
    }
}