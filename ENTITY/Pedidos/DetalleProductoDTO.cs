using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Pedidos
{
    public class DetalleProductoDTO
    {
        public int IdDetalle { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubtotalLinea { get; set; }
        public string NombreProducto { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Talla { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string CodigoSKU { get; set; } = null!;
        public string? ImagenUrl { get; set; }
    }
}
