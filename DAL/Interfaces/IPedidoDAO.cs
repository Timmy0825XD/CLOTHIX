using ENTITY.Pedidos;
using ENTITY.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IPedidoDAO
    {

        Task<Response<int>> CrearPedido(CrearPedidoDTO pedido);

        Task<Response<PedidoDTO>> ObtenerPedidosUsuario(int idUsuario);

        Task<Response<PedidoCompletoDTO>> ObtenerPedidoCompleto(int idPedido);

        Task<Response<PedidoListaDTO>> ObtenerTodosPedidos();

        Task<Response<bool>> ActualizarEstadoPedido(ActualizarEstadoPedidoDTO actualizacion);

        Task<Response<bool>> CancelarPedido(int idPedido);

        Task<Response<MetodoPagoDTO>> ObtenerMetodosPago();
    }
}
