namespace microInventario.API.Model.Request
{
    public class AgregarProductoRequest
    {
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Image { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public int Peso { get; set; }
        public byte[] ImageBase64 { get; set; } = new byte[] { };
    }
}
