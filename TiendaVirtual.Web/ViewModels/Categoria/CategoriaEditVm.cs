using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Web.ViewModels.Categoria
{
    public class CategoriaEditVm
    {
        public int CategoriaId { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string NombreCategoria { get; set; }
        [StringLength(255, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]

        public string Descripcion { get; set; }
        public byte[] RowVersion { get; set; }
    }
}