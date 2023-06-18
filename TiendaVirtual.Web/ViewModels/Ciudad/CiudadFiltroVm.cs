using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Profile;

namespace TiendaVirtual.Web.ViewModels.Ciudad
{
    public class CiudadFiltroVm
    {
        public List<SelectListItem> Paises { get; set; }
        public List<CiudadListVm> Ciudades { get; set; }
        public int? PaisFiltro { get; set; }
    }
}