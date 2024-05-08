namespace Pruba_tienda_api.DTO
{
    public class ProductoAdmDTO
    {
        public int ProductoId { get; set; }
        public string? NombreProducto { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string? NombreCategoria { get; set; }
    }
}
