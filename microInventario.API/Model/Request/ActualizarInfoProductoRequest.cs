namespace microInventario.API.Model.Request
{
    public class ActualizarInfoProductoRequest
    {
        public int IdMarca { get; set; } = 0;
        public int IdCategoria { get; set; } = 0;
        public string Descripcion { get; set; } = string.Empty;
        public string Nombre { get; set; } = String.Empty;
        public string Image { get; set; }
        public byte[] ImageBase64 { get; set; } = new byte[0];
        public decimal Precio { get; set; } = decimal.Zero;
        public int Cantidad { get; set; } = 0;
        public int Peso { get; set; } = 0;
    }
}
