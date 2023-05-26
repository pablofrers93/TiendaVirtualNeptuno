using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Web.ViewModels.Categoria
{
    public class CategoriaListVm
    {
        public int CategoriaId { get; set; }
        [DisplayName("Categoría")]
        public string NombreCategoria { get; set; }
        public string Descripcion { get; set; }
        public byte[] RowVersion { get; set; }
    }
}