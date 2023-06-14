using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        [Range(1, int.MaxValue, ErrorMessage ="Debe seleccionar un país")]
        public int CiudadId { get; set; }
        public string CodPostal { get; set; }
        [DisplayName("Tel.Movil")]
        [MaxLength(20, ErrorMessage ="El campo {0} no puede tener más de 20 caracteres")]
        public string TelefonoMovil { get; set; }

    }
}