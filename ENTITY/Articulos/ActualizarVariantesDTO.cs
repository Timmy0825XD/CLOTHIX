using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Articulos
{
    public class ActualizarVariantesDTO
    {
        public int IdArticulo { get; set; }
        public List<VarianteActualizacionDTO> Variantes { get; set; } = new();
    }
}
