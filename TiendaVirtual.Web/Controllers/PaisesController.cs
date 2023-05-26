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
                var paisVm = GetPaisListVm(item);
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
                NombrePais = paisEditVm.NombrePais,
                RowVersion = paisEditVm.RowVersion
            };
        }
        private PaisListVm GetPaisListVm(Pais pais)
        {
            return new PaisListVm()
            {
                PaisId = pais.PaisId,
                NombrePais = pais.NombrePais
            };
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var pais = _servicio.GetPaisPorId(id.Value);
            if (pais == null)
            {
                return HttpNotFound("Codigo de país inexistente");
            }
            var paisVm = GetPaisListVm(pais);
            return View(paisVm);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            var pais = _servicio.GetPaisPorId(id);
            if (_servicio.EstaRelacionado(pais))
            {
                var paisVm = GetPaisListVm(pais);
                ModelState.AddModelError(string.Empty, "País relacionado... Baja denegada");
                return View(paisVm);
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
            var pais = _servicio.GetPaisPorId(id.Value);
            if (pais == null)
            {
                return HttpNotFound("Codigo de país inexistente");
            }
            var paisVm = GetPaisEditVmFromPais(pais);
            return View(paisVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaisEditVm paisVm) 
        {
            if (!ModelState.IsValid)
            {
                return View(paisVm);
            }
            var pais = GetPaisFromPaisEditVm(paisVm);
            if (_servicio.Existe(pais))
            {
                ModelState.AddModelError(string.Empty, "Pais Existente");
                return View(paisVm);
            }
            _servicio.Guardar(pais);
            TempData["Msg"] = "Registro editado satisfactoriamente";
            return RedirectToAction("Index");
        }
        private PaisEditVm GetPaisEditVmFromPais(Pais pais)
        {
            return new PaisEditVm()
            {
                PaisId = pais.PaisId,
                NombrePais = pais.NombrePais,
                RowVersion = pais.RowVersion
            };
        }
    }
}