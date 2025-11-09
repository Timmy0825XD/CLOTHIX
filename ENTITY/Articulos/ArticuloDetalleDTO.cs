using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Articulos
{
    public class ArticuloDetalleDTO
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Marca { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public string? Material { get; set; }
        public decimal PrecioBase { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public int CategoriaTipoId { get; set; }
        public int CategoriaOcasionId { get; set; }
        public string CategoriaTipo { get; set; } = null!;
        public string CategoriaOcasion { get; set; } = null!;
        public List<VarianteDetalleDTO> Variantes { get; set; } = new();
        public List<ImagenArticuloDTO> Imagenes { get; set; } = new();
    }
}
