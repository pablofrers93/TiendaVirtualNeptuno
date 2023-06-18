using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TiendaVirtual.Datos;
using TiendaVirtual.Datos.Interfaces;
using TiendaVirtual.Entidades.Dtos.Ciudad;
using TiendaVirtual.Entidades.Entidades;
using TiendaVirtual.Servicios.Interfaces;

namespace Neptuno2022EF.Servicios.Servicios
{
    public class ServiciosCiudades : IServiciosCiudades
    {
        private readonly IRepositorioCiudades _repositorioCiudades;
        private readonly IUnitOfWork _unitOfWork;


        public ServiciosCiudades(IRepositorioCiudades repositorioCiudades, IUnitOfWork unitOfWork)
        {
            _repositorioCiudades = repositorioCiudades;
            _unitOfWork = unitOfWork;
        }

        public void Borrar(int ciudadId)
        {
            try
            {
                _repositorioCiudades.Borrar(ciudadId);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool EstaRelacionada(Ciudad ciudad)
        {
            try
            {
                return _repositorioCiudades.EstaRelacionada(ciudad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Existe(Ciudad ciudad)
        {
            try
            {
                return _repositorioCiudades.Existe(ciudad);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CiudadListDto> GetCiudades()
        {
            try
            {
                return _repositorioCiudades.GetCiudades();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Ciudad GetCiudadPorId(int ciudadId)
        {
            try
            {
                return _repositorioCiudades.GetCiudadPorId(ciudadId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CiudadListDto> GetCiudades(int paisId)
        {
            try
            {
                return _repositorioCiudades.GetCiudades(paisId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Guardar(Ciudad ciudad)
        {
            try
            {
                if (ciudad.CiudadId==0)
                {
                    _repositorioCiudades.Agregar(ciudad);
                   
                }
                else
                {
                    _repositorioCiudades.Editar(ciudad);
                }
                _unitOfWork.SaveChanges();
                

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CiudadListDto> Filtrar(Func<Ciudad, bool> predicado, int cantidad, int pagina)
        {
            try
            {
                return _repositorioCiudades.Filtrar(predicado, cantidad, pagina);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCantidad()
        {
            try
            {
                return _repositorioCiudades.GetCantidad();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CiudadListDto> GetCiudadesPorPagina(int cantidad, int pagina)
        {
            try
            {
                return _repositorioCiudades.GetCiudadesPorPagina(cantidad, pagina);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetCantidad(Func<Ciudad, bool> predicado)
        {
            try
            {
                return _repositorioCiudades.GetCantidad(predicado);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SelectListItem> GetCiudadesDropDownList(int paisId)
        {
            try
            {
                return _repositorioCiudades.GetCiudadesDropDownList(paisId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
