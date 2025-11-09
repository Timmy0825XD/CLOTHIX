using ENTITY.Articulos;
using ENTITY.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICategoriaDAO
    {
        Task<Response<CategoriaTipoDTO>> ObtenerCategoriasTipo();
        Task<Response<CategoriaOcasionDTO>> ObtenerCategoriasOcasion();
    }
}
