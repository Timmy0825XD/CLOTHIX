using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Articulos
{
    public class ActualizarImagenesDTO
    {
        public int IdArticulo { get; set; }
        public List<ImagenActualizacionDTO> Imagenes { get; set; } = new();
    }
}
