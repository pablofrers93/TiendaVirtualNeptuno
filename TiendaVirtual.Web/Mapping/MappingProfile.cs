using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Dtos.Cliente;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Web.ViewModels.Ciudad;
using TiendaVirtual.Web.ViewModels.Cliente;
using TiendaVirtual.Web.ViewModels.Pais;

namespace TiendaVirtual.Web.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            LoadPaisesMapping();
            LoadCiudadesMapping();
            LoadClientesMapping();
        }

        private void LoadClientesMapping()
        {
            CreateMap<ClienteListDto, ClienteListVm>();
            CreateMap<Cliente, ClienteEditVm>().ReverseMap();
            CreateMap<Cliente, ClienteListVm>()
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreCliente, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais.NombrePais))
                .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad.NombreCiudad)); 
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