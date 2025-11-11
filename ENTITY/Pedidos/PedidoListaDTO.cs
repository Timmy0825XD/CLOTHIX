using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY.Pedidos
{
    public class PedidoListaDTO
    {
        public int IdPedido { get; set; }
        public string NumeroPedido { get; set; } = null!;
        public DateTime FechaPedido { get; set; }
        public string Estado { get; set; } = null!;
        public decimal Total { get; set; }
        public string NombreCliente { get; set; } = null!;
        public string CorreoCliente { get; set; } = null!;
        public string MetodoPago { get; set; } = null!;
        public int TotalProductos { get; set; }
    }
}
