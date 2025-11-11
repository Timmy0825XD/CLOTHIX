using ENTITY.Pedidos;

namespace GUI.Services
{
    public class CarritoService
    {
        private List<CarritoItemDTO> _items = new();

        // Evento que se dispara cuando cambia el carrito
        public event Action? OnChange;

        // ========================================
        // AGREGAR ITEM AL CARRITO
        // ========================================
        public void AgregarItem(CarritoItemDTO item)
        {
            // Verificar si el producto ya existe en el carrito
            var itemExistente = _items.FirstOrDefault(i => i.IdVariante == item.IdVariante);

            if (itemExistente != null)
            {
                // Si existe, aumentar la cantidad
                itemExistente.Cantidad += item.Cantidad;

                // Validar que no exceda el stock
                if (itemExistente.Cantidad > itemExistente.StockDisponible)
                {
                    itemExistente.Cantidad = itemExistente.StockDisponible;
                }
            }
            else
            {
                // Si no existe, agregarlo
                _items.Add(item);
            }

            NotificarCambio();
        }

        // ========================================
        // ACTUALIZAR CANTIDAD
        // ========================================
        public void ActualizarCantidad(int idVariante, int nuevaCantidad)
        {
            var item = _items.FirstOrDefault(i => i.IdVariante == idVariante);

            if (item != null)
            {
                // Validar cantidad mínima y máxima
                if (nuevaCantidad <= 0)
                {
                    EliminarItem(idVariante);
                    return;
                }

                if (nuevaCantidad > item.StockDisponible)
                {
                    nuevaCantidad = item.StockDisponible;
                }

                if (nuevaCantidad > 99)
                {
                    nuevaCantidad = 99;
                }

                item.Cantidad = nuevaCantidad;
                NotificarCambio();
            }
        }

        // ========================================
        // ELIMINAR ITEM
        // ========================================
        public void EliminarItem(int idVariante)
        {
            _items.RemoveAll(i => i.IdVariante == idVariante);
            NotificarCambio();
        }

        // ========================================
        // LIMPIAR CARRITO
        // ========================================
        public void Limpiar()
        {
            _items.Clear();
            NotificarCambio();
        }

        // ========================================
        // OBTENER ITEMS
        // ========================================
        public List<CarritoItemDTO> ObtenerItems()
        {
            return _items.ToList();
        }

        // ========================================
        // OBTENER CANTIDAD TOTAL DE ITEMS
        // ========================================
        public int ObtenerCantidadTotal()
        {
            return _items.Sum(i => i.Cantidad);
        }

        // ========================================
        // CALCULAR SUBTOTAL
        // ========================================
        public decimal ObtenerSubtotal()
        {
            return _items.Sum(i => i.Subtotal);
        }

        // ========================================
        // CALCULAR IVA (19%)
        // ========================================
        public decimal ObtenerImpuesto()
        {
            return ObtenerSubtotal() * 0.19m;
        }

        // ========================================
        // CALCULAR TOTAL
        // ========================================
        public decimal ObtenerTotal()
        {
            return ObtenerSubtotal() + ObtenerImpuesto();
        }

        // ========================================
        // VERIFICAR SI EL CARRITO ESTÁ VACÍO
        // ========================================
        public bool EstaVacio()
        {
            return !_items.Any();
        }

        // ========================================
        // OBTENER CANTIDAD DE UN PRODUCTO ESPECÍFICO
        // ========================================
        public int ObtenerCantidadProducto(int idVariante)
        {
            var item = _items.FirstOrDefault(i => i.IdVariante == idVariante);
            return item?.Cantidad ?? 0;
        }

        // ========================================
        // VERIFICAR SI UN PRODUCTO ESTÁ EN EL CARRITO
        // ========================================
        public bool ContieneProducto(int idVariante)
        {
            return _items.Any(i => i.IdVariante == idVariante);
        }

        // ========================================
        // NOTIFICAR CAMBIOS
        // ========================================
        private void NotificarCambio()
        {
            OnChange?.Invoke();
        }
    }
}
