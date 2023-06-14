using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual.Web.ViewModels.Cliente
{
    public class ClienteEditVm
    {
        public int Id { get; set; }
        [DisplayName("Cliente")]
        [Required(ErrorMessage= "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage= "El campo {0} debe contener entre 3 y 100 caracteres")]

        public string Nombre { get; set; }
        [DisplayName("Direccion")]
        [Required(ErrorMessage="El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} debe contener entre 3 y 100 caracteres")]
        public string Direccion { get; set; }
        [DisplayName("Pais")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un país")]

        public int PaisId { get; set; }
        [DisplayName("Ciudad")]
        [Range(1, int.MaxValue, ErrorMessage ="Debe seleccionar un país")]
        public int CiudadId { get; set; }
        public string CodPostal { get; set; }
        [DisplayName("Tel. Fijo")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
        public string TelefonoFijo { get; set; }
        [DisplayName("Tel. Móvil")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]

        public string TelefonoMovil { get; set; }
        public byte[] RowVersion { get; set; }
        public List<SelectListItem> Paises { get; set; }
        public List<SelectListItem> Ciudades { get; set; }        
    }
}