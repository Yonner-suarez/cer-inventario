namespace microInventario.API.Model.Response
{
    public class ProductoResponse
    {
        public MarcaResponse Marca { get; set; }
        public CategoriaResponse Categoria { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public byte[] Image { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
