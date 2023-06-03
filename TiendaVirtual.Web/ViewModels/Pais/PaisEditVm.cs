﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Web.ViewModels.Pais
{
    public class PaisEditVm
    {
        public int PaisId { get; set; }
        [DisplayName("País")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres", MinimumLength = 3)]
        public string NombrePais { get; set; }

        public byte[] RowVersion { get; set; }
    }
}