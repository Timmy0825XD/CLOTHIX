namespace ENTITY.Articulos
{
    public class VarianteDetalleDTO
    {
        public int IdVariante { get; set; }
        public string Talla { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string CodigoSKU { get; set; } = null!;
        public int Stock { get; set; }
        public string Estado { get; set; } = null!;
    }
}