using BLL.Interfaces;
using DAL.Interfaces;
using ENTITY.Pedidos;
using ENTITY.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementaciones
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDAO _pedidoDAO;

        public PedidoService(IPedidoDAO pedidoDAO)
        {
            _pedidoDAO = pedidoDAO;
        }

        // ========================================
        // 1. CREAR PEDIDO
        // ========================================
        public async Task<Response<int>> CrearPedido(CrearPedidoDTO pedido)
        {
            try
            {
                // Validaciones de negocio
                if (pedido.IdUsuario <= 0)
                {
                    return Response<int>.Fail("ID de usuario inválido");
                }

                if (pedido.IdDireccionEnvio <= 0)
                {
                    return Response<int>.Fail("Debe seleccionar una dirección de envío");
                }

                if (pedido.IdMetodoPago <= 0)
                {
                    return Response<int>.Fail("Debe seleccionar un método de pago");
                }

                if (pedido.Productos == null || !pedido.Productos.Any())
                {
                    return Response<int>.Fail("El pedido debe contener al menos un producto");
                }

                // Validar cada producto
                foreach (var producto in pedido.Productos)
                {
                    if (producto.IdVariante <= 0)
                    {
                        return Response<int>.Fail("ID de variante inválido");
                    }

                    if (producto.Cantidad <= 0)
                    {
                        return Response<int>.Fail("La cantidad debe ser mayor a 0");
                    }

                    if (producto.Cantidad > 99)
                    {
                        return Response<int>.Fail("La cantidad máxima por producto es 99");
                    }
                }

                // Validar productos duplicados
                var productosDuplicados = pedido.Productos
                    .GroupBy(p => p.IdVariante)
                    .Where(g => g.Count() > 1)
                    .ToList();

                if (productosDuplicados.Any())
                {
                    return Response<int>.Fail("El pedido contiene productos duplicados");
                }

                // Llamar al DAO
                return await _pedidoDAO.CrearPedido(pedido);
            }
            catch (Exception ex)
            {
                return Response<int>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 2. OBTENER PEDIDOS POR USUARIO
        // ========================================
        public async Task<Response<PedidoDTO>> ObtenerPedidosUsuario(int idUsuario)
        {
            try
            {
                if (idUsuario <= 0)
                {
                    return Response<PedidoDTO>.Fail("ID de usuario inválido");
                }

                return await _pedidoDAO.ObtenerPedidosUsuario(idUsuario);
            }
            catch (Exception ex)
            {
                return Response<PedidoDTO>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 3. OBTENER PEDIDO COMPLETO
        // ========================================
        public async Task<Response<PedidoCompletoDTO>> ObtenerPedidoCompleto(int idPedido)
        {
            try
            {
                if (idPedido <= 0)
                {
                    return Response<PedidoCompletoDTO>.Fail("ID de pedido inválido");
                }

                return await _pedidoDAO.ObtenerPedidoCompleto(idPedido);
            }
            catch (Exception ex)
            {
                return Response<PedidoCompletoDTO>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 4. OBTENER TODOS LOS PEDIDOS
        // ========================================
        public async Task<Response<PedidoListaDTO>> ObtenerTodosPedidos()
        {
            try
            {
                return await _pedidoDAO.ObtenerTodosPedidos();
            }
            catch (Exception ex)
            {
                return Response<PedidoListaDTO>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 5. ACTUALIZAR ESTADO DEL PEDIDO
        // ========================================
        public async Task<Response<bool>> ActualizarEstadoPedido(ActualizarEstadoPedidoDTO actualizacion)
        {
            try
            {
                if (actualizacion.IdPedido <= 0)
                {
                    return Response<bool>.Fail("ID de pedido inválido");
                }

                if (string.IsNullOrWhiteSpace(actualizacion.Estado))
                {
                    return Response<bool>.Fail("El estado es requerido");
                }

                // Validar estados permitidos
                var estadosPermitidos = new[] { "Pendiente", "Confirmado", "Enviado", "Entregado", "Cancelado" };
                if (!estadosPermitidos.Contains(actualizacion.Estado))
                {
                    return Response<bool>.Fail($"Estado inválido. Valores permitidos: {string.Join(", ", estadosPermitidos)}");
                }

                return await _pedidoDAO.ActualizarEstadoPedido(actualizacion);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 6. CANCELAR PEDIDO
        // ========================================
        public async Task<Response<bool>> CancelarPedido(int idPedido)
        {
            try
            {
                if (idPedido <= 0)
                {
                    return Response<bool>.Fail("ID de pedido inválido");
                }

                return await _pedidoDAO.CancelarPedido(idPedido);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }

        // ========================================
        // 7. OBTENER MÉTODOS DE PAGO
        // ========================================
        public async Task<Response<MetodoPagoDTO>> ObtenerMetodosPago()
        {
            try
            {
                return await _pedidoDAO.ObtenerMetodosPago();
            }
            catch (Exception ex)
            {
                return Response<MetodoPagoDTO>.Fail($"Error en la capa de negocio: {ex.Message}");
            }
        }
    }
}
