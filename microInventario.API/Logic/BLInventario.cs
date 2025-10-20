using microInventario.API.Dao;
using microInventario.API.Model;
using microInventario.API.Model.Request;
using microInventario.API.Model.Response;
using microInventario.API.Utils;

namespace microInventario.API.BL
{
    public class BLInventario
    {
        public static GeneralResponse AgregarProducto(AgregarProductoRequest request, int idAdmin)
        {
            byte[] imagenBytes = Convert.FromBase64String(request.Image);
            request.ImageBase64 = imagenBytes;
            var res = DAInventaro.InsertarProducto(request, idAdmin);
            return res;
        }
        public static GeneralResponse ActualizarCantidad(int idproducto, int nuevaCantidad, int idAdmin)
        {
            var res = DAInventaro.ActualizarCantidadProducto(idproducto, nuevaCantidad, idAdmin);
            return res;
        }
        public static GeneralResponse EliminarProducto(int idproducto, int idAdmin)
        {
            var res = DAInventaro.EliminarProducto(idproducto, idAdmin);
            return res;
        }

        public static GeneralResponse ObtenerProducto(int idproducto)
        {
            var res = DAInventaro.ObtenerProducto(idproducto);
            if (res is null) return new GeneralResponse
            {
                data = null,
                message = "No se encontó el producto",
                status = Variables.Response.BadRequest
            };


            return new GeneralResponse
            {
                data = res,
                message = "La tarea se completo con exito",
                status = Variables.Response.OK,
            };
        }

        public static GeneralResponse ObtenerProductos()
        {
            var res = DAInventaro.ObtenerProductos();
            return res;
        }

        public static GeneralResponse ActualizarProductoData(ActualizarInfoProductoRequest req, int idProducto, int idAdmin)
        {
            // Obtener el producto actual
            ProductoResponse producto = DAInventaro.ObtenerProducto(idProducto);
            if (producto is null)
            {
                return new GeneralResponse
                {
                    data = null,
                    message = "No se encontró el producto",
                    status = Variables.Response.BadRequest
                };
            }

            // Actualizar solo los campos que sean distintos de cero, vacío o null
            if (req.IdMarca != 0 && req.IdMarca != producto.Marca.IdMarca)
                producto.Marca.IdMarca = req.IdMarca;

            if (req.IdCategoria != 0 && req.IdCategoria != producto.Categoria.IdCategoria)
                producto.Categoria.IdCategoria = req.IdCategoria;

            if (!string.IsNullOrEmpty(req.Nombre) && req.Nombre != producto.Nombre)
                producto.Nombre = req.Nombre;

            if (!string.IsNullOrEmpty(req.Descripcion) && req.Descripcion != producto.Descripcion)
                producto.Descripcion = req.Descripcion;

            if (req.Precio != 0 && req.Precio != producto.Precio)
                producto.Precio = req.Precio;

            if (req.Cantidad != 0 && req.Cantidad != producto.Cantidad)
                producto.Cantidad = req.Cantidad;

            if (req.Image != null && req.Image.Length > 0 )
            {
                producto.Image = Convert.FromBase64String(req.Image);
            }

            // Llamar al método que actualiza el producto completo
            var res = DAInventaro.ActualizarProducto(idProducto, producto, idAdmin);
            return res;
        }
        public static GeneralResponse ActualizarStockCuandoPagoConfirmnado(List<ActualizarStockProducto> request)
        {
            var res = DAInventaro.ActualizarStockCuandoPagoConfirmado(request);
            return res;
        }
    }
}
