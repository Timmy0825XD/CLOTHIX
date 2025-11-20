using ENTITY.Favoritos;
using ENTITY.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IFavoritoDAO
    {
        
        Task<Response<bool>> EliminarFavorito(int idUsuario, int idArticulo);
        Task<Response<ToggleFavoritoResultDTO>> ToggleFavorito(int idUsuario, int idArticulo);
        Task<Response<FavoritoDTO>> ObtenerFavoritosUsuario(int idUsuario);
    }
}
