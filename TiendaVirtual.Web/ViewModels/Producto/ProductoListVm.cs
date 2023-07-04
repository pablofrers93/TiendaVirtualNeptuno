﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TiendaVirtual.Entidades.Entidades;

namespace TiendaVirtual.Web.ViewModels.Producto
{
    public class ProductoListVm
    {
        public int ProductoId { get; set; }
        [DisplayName("Producto")]
        public string NombreProducto { get; set; }
        [DisplayName("Categoría")]
        public string Categoria { get; set; }

        [DisplayName("P. Unit")]
        public decimal PrecioUnitario { get; set; }
        [DisplayName("Stock")]
        public int UnidadesDisponibles { get; set; }
        public bool Suspendido { get; set; }
        [DisplayName("Imágen")]
        public string Imagen { get; set; }
    }   
}