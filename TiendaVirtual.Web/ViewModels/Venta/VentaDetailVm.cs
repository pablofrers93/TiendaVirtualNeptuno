using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaVirtual.Web.ViewModels.DetalleVenta;

namespace TiendaVirtual.Web.ViewModels.Venta
{
    public class VentaDetailVm
    {
        public VentaListVm Venta { get; set; }
        public List<DetalleVentaListVm> Detalles { get; set; }
    }
}