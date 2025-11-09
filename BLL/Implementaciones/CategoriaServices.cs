using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY.Articulos;
using ENTITY.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementaciones
{
    public class CategoriaServices : ICategoriaServices
    {
        private readonly ICategoriaDAO _categoriaDAO;

        public CategoriaServices(ICategoriaDAO categoriaDAO)
        {
            _categoriaDAO = categoriaDAO;
        }

        // ========================================
        // 1. OBTENER CATEGORÍAS TIPO
        // ========================================
        public async Task<Response<CategoriaTipoDTO>> ObtenerCategoriasTipo()
        {
            try
            {
                return await _categoriaDAO.ObtenerCategoriasTipo();
            }
            catch (Exception ex)
            {
                return Response<CategoriaTipoDTO>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 2. OBTENER CATEGORÍAS OCASIÓN
        // ========================================
        public async Task<Response<CategoriaOcasionDTO>> ObtenerCategoriasOcasion()
        {
            try
            {
                return await _categoriaDAO.ObtenerCategoriasOcasion();
            }
            catch (Exception ex)
            {
                return Response<CategoriaOcasionDTO>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }
    }
}
