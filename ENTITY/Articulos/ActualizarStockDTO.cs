using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Articulos
{
    public class ActualizarStockDTO
    {
        public int IdArticulo { get; set; }
        public string Talla { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int NuevoStock { get; set; }
    }
}
