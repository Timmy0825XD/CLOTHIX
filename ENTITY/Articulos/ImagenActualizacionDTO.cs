using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Articulos
{
    public class ImagenActualizacionDTO
    {
        public int? IdImagen { get; set; }
        public string Url { get; set; } = null!;
        public int Orden { get; set; }
        public char EsPrincipal { get; set; }
    }
}
