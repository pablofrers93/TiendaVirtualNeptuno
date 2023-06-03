using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Pais;

namespace TiendaVirtual.Web.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            LoadPaisesMapping();
            LoadCiudadesMapping();
        }

        private void LoadCiudadesMapping()
        {
            CreateMap<CiudadListDto, CiudadListVm>();
            CreateMap<CiudadEditVm, Ciudad>().ReverseMap();
            CreateMap<Ciudad, CiudadListVm>().ForMember(dest=>dest.NombrePais,
                opt=>opt.MapFrom(src=>src.Pais.NombrePais)).ReverseMap();
        }

        private void LoadPaisesMapping()
        {
            CreateMap<Pais, PaisListVm>();
            CreateMap<Pais, PaisEditVm>().ReverseMap();             
        }
    }
}