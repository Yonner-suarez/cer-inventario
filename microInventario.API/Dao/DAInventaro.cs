using microInventario.API.Model.Request;
using microInventario.API.Model;
using microInventario.API.Util;
using MySql.Data.MySqlClient;
using microInventario.API.Utils;
using microInventario.API.Model.Response;

namespace microInventario.API.Dao
{
    public static class DAInventaro
    {
        public static ProductoResponse ObtenerProducto(int idProducto)
        {
            ProductoResponse producto = null;

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlSelect = @"
                                        SELECT 
                                            p.cer_int_id_marca,
                                            m.cer_enum_nombre AS MarcaNombre,
                                            p.cer_int_id_categoria,
                                            c.cer_enum_nombre AS CategoriaNombre,
                                            p.cer_text_descripcion,
                                            p.cer_varchar_nombre,
                                            p.cer_blob_imagen,
                                            p.cer_decimal_precio,
                                            p.cer_int_stock
                                        FROM tbl_cer_producto p
                                        INNER JOIN tbl_cer_marca m ON p.cer_int_id_marca = m.cer_int_id_marca
                                        INNER JOIN tbl_cer_categoria c ON p.cer_int_id_categoria = c.cer_int_id_categoria
                                        WHERE p.cer_int_id_producto = @IdProducto
                                          AND p.cer_tinyint_estado = 1;
                                    ";
                    using (var cmd = new MySqlCommand(sqlSelect, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                producto = new ProductoResponse
                                {
                                    Marca = new MarcaResponse
                                    {
                                        IdMarca = Convert.ToInt32(reader["cer_int_id_marca"]),
                                        Nombre = reader["MarcaNombre"]?.ToString() ?? string.Empty,
                                    },
                                    Categoria = new CategoriaResponse 
                                    {
                                        IdCategoria = reader["cer_int_id_categoria"] != DBNull.Value ? Convert.ToInt32(reader["cer_int_id_categoria"]) : 0,
                                        Nombre = reader["CategoriaNombre"]?.ToString() ?? string.Empty,
                                    },
                                    Descripcion = reader["cer_text_descripcion"]?.ToString() ?? string.Empty,
                                    Nombre = reader["cer_varchar_nombre"]?.ToString() ?? string.Empty,
                                    Image = reader["cer_blob_imagen"] != DBNull.Value ? (byte[])reader["cer_blob_imagen"] : null,
                                    Precio = Convert.ToDecimal(reader["cer_decimal_precio"]),
                                    Cantidad = Convert.ToInt32(reader["cer_int_stock"])
                                };
                            }
                        }
                    }
                }
                catch
                {
                    producto = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return producto;
        }


        public static GeneralResponse InsertarProducto(AgregarProductoRequest request, int IdAdmin)
        {
            var response = new GeneralResponse();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlInsert = @"
                        INSERT INTO tbl_cer_producto
                        (cer_varchar_nombre, cer_text_descripcion, cer_decimal_precio, cer_int_stock, 
                         cer_int_id_marca, cer_int_id_categoria, cer_datetime_created_at, cer_int_created_by, cer_blob_imagen)
                        VALUES
                        (@Nombre, @Descripcion, @Precio, @Cantidad, @IdMarca, @IdCategoria, NOW(), @IdAdmin, @Imagen);
                        SELECT LAST_INSERT_ID();";

                    var cmd = new MySqlCommand(sqlInsert, conn);

                    cmd.Parameters.AddWithValue("@Nombre", request.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(request.Descripcion) ? (object)DBNull.Value : request.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", request.Precio);
                    cmd.Parameters.AddWithValue("@Cantidad", request.Cantidad);
                    cmd.Parameters.AddWithValue("@IdMarca", request.IdMarca);
                    cmd.Parameters.AddWithValue("@IdCategoria", request.IdCategoria != 0 ? request.IdCategoria : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdAdmin", IdAdmin);
                    cmd.Parameters.AddWithValue("@Imagen", request.ImageBase64 != null ? request.ImageBase64 : (object)DBNull.Value);

                    // Ejecutar y obtener Id insertado
                    int nuevoId = Convert.ToInt32(cmd.ExecuteScalar());

                    response.status = Variables.Response.OK;
                    response.message = "Producto insertado correctamente.";
                    response.data = nuevoId;
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al insertar producto: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse ActualizarCantidadProducto(int idProducto, int nuevaCantidad, int idAdmin)
        {
            var response = new GeneralResponse();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlUpdate = @"
                                        UPDATE tbl_cer_producto
                                        SET cer_int_stock = @NuevaCantidad,
                                            cer_int_updated_by = @IdAdmin,
                                            cer_datetime_updated_at = NOW()
                                        WHERE cer_int_id_producto = @IdProducto;
                                    ";

                    var cmd = new MySqlCommand(sqlUpdate, conn);

                    cmd.Parameters.AddWithValue("@NuevaCantidad", nuevaCantidad);
                    cmd.Parameters.AddWithValue("@IdAdmin", idAdmin);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        response.status = Variables.Response.OK;
                        response.message = "Cantidad del producto actualizada correctamente.";
                        response.data = filasAfectadas;
                    }
                    else
                    {
                        response.status = Variables.Response.BadRequest;
                        response.message = "No se encontró el producto con el ID proporcionado.";
                        response.data = null;
                    }
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al actualizar cantidad del producto: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse EliminarProducto(int idProducto, int idAdmin)
        {
            var response = new GeneralResponse();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlDeleteLogico = @"
                                            UPDATE tbl_cer_producto
                                            SET cer_datetime_deleted_at = NOW(),
                                                cer_int_updated_by = @IdAdmin,
                                                cer_tinyint_estado = 0
                                            WHERE cer_int_id_producto = @IdProducto
                                              AND cer_tinyint_estado = 1;
                                        ";

                    var cmd = new MySqlCommand(sqlDeleteLogico, conn);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.Parameters.AddWithValue("@IdAdmin", idAdmin);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        response.status = Variables.Response.OK;
                        response.message = "Producto eliminado correctamente (borrado lógico).";
                        response.data = filasAfectadas;
                    }
                    else
                    {
                        response.status = Variables.Response.BadRequest;
                        response.message = "No se encontró el producto activo con el ID proporcionado.";
                        response.data = null;
                    }
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al eliminar el producto: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse ActualizarProducto(int idProducto, ProductoResponse producto, int idAdmin)
        {
            var response = new GeneralResponse();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlUpdate = @"
                        UPDATE tbl_cer_producto
                        SET cer_varchar_nombre = @Nombre,
                            cer_text_descripcion = @Descripcion,
                            cer_decimal_precio = @Precio,
                            cer_int_stock = @Cantidad,
                            cer_int_id_marca = @Marca,
                            cer_int_id_categoria = @Categoria,
                            cer_blob_imagen = @Imagen,
                            cer_int_updated_by = @IdAdmin,
                            cer_datetime_updated_at = NOW()
                        WHERE cer_int_id_producto = @IdProducto
                          AND cer_tinyint_estado = 1;
                    ";

                    var cmd = new MySqlCommand(sqlUpdate, conn);
                    cmd.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                    cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                    cmd.Parameters.AddWithValue("@Cantidad", producto.Cantidad);
                    cmd.Parameters.AddWithValue("@Marca", producto.Marca.IdMarca != 0 ? producto.Marca.IdMarca : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Categoria", producto.Categoria.IdCategoria != 0 ? producto.Categoria.IdCategoria : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Imagen", producto.Image != null ? producto.Image : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdAdmin", idAdmin);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    response.status = filasAfectadas > 0 ? Variables.Response.OK : Variables.Response.BadRequest;
                    response.message = filasAfectadas > 0 ? "Producto actualizado correctamente." : "No se encontró el producto o no hubo cambios.";
                    response.data = filasAfectadas;
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al actualizar producto: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse ObtenerProductos()
        {
            var response = new GeneralResponse();
            var listaProductos = new List<ProductoResponse>();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    string sqlSelect = @"
                                        SELECT 
                                            p.cer_int_id_producto,
                                            p.cer_int_id_marca,
                                            m.cer_enum_nombre AS MarcaNombre,
                                            p.cer_int_id_categoria,
                                            c.cer_enum_nombre AS CategoriaNombre,
                                            p.cer_varchar_nombre,
                                            p.cer_text_descripcion,
                                            p.cer_decimal_precio,
                                            p.cer_int_stock,
                                            p.cer_blob_imagen
                                        FROM tbl_cer_producto p
                                        INNER JOIN tbl_cer_marca m ON p.cer_int_id_marca = m.cer_int_id_marca
                                        INNER JOIN tbl_cer_categoria c ON p.cer_int_id_categoria = c.cer_int_id_categoria
                                        WHERE p.cer_tinyint_estado = 1;
                                    ";

                    using (var cmd = new MySqlCommand(sqlSelect, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var producto = new ProductoResponse
                                {
                                    Marca = new MarcaResponse
                                    {
                                        IdMarca = Convert.ToInt32(reader["cer_int_id_marca"]),
                                        Nombre = reader["MarcaNombre"]?.ToString() ?? string.Empty,
                                    },
                                    Categoria = new CategoriaResponse
                                    {
                                        IdCategoria = reader["cer_int_id_categoria"] != DBNull.Value ? Convert.ToInt32(reader["cer_int_id_categoria"]) : 0,
                                        Nombre = reader["CategoriaNombre"]?.ToString() ?? string.Empty,
                                    },
                                    Nombre = reader["cer_varchar_nombre"]?.ToString() ?? string.Empty,
                                    Descripcion = reader["cer_text_descripcion"]?.ToString() ?? string.Empty,
                                    Precio = Convert.ToDecimal(reader["cer_decimal_precio"]),
                                    Cantidad = Convert.ToInt32(reader["cer_int_stock"]),
                                    Image = reader["cer_blob_imagen"] != DBNull.Value ? (byte[])reader["cer_blob_imagen"] : null,
                                    IdProducto = Convert.ToInt32(reader["cer_int_id_producto"])
                                };

                                listaProductos.Add(producto);
                            }
                        }
                    }

                    response.status = Variables.Response.OK;
                    response.message = listaProductos.Count > 0 ? "Productos obtenidos correctamente." : "No se encontraron productos activos.";
                    response.data = listaProductos;
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al obtener productos: " + ex.Message;
                    response.data = null;
                }
                finally
                {
                    conn.Close();
                }
            }

            return response;
        }
        public static GeneralResponse ActualizarStockCuandoPagoConfirmado(List<ActualizarStockProducto> request)
        {
            var response = new GeneralResponse();

            using (MySqlConnection conn = new MySqlConnection(Variables.Conexion.cnx))
            {
                try
                {
                    conn.Open();

                    int totalActualizados = 0;

                    foreach (var item in request)
                    {
                        string sqlUpdate = @"
                                UPDATE tbl_cer_producto
                                SET cer_int_stock = cer_int_stock - @Cantidad,
                                    cer_datetime_updated_at = NOW()
                                WHERE cer_int_id_producto = @IdProducto;
                            ";

                        using (var cmd = new MySqlCommand(sqlUpdate, conn))
                        {
                            cmd.Parameters.AddWithValue("@Cantidad", item.Cantidad);
                            cmd.Parameters.AddWithValue("@IdProducto", item.IdProducto);

                            int filasAfectadas = cmd.ExecuteNonQuery();
                            totalActualizados += filasAfectadas;
                        }
                    }

                    if (totalActualizados > 0)
                    {
                        response.status = Variables.Response.OK;
                        response.message = "Stock actualizado correctamente.";
                        response.data = totalActualizados;
                    }
                    else
                    {
                        response.status = Variables.Response.BadRequest;
                        response.message = "No se actualizaron productos. Verifica los IDs proporcionados.";
                        response.data = null;
                    }
                }
                catch (Exception ex)
                {
                    response.status = Variables.Response.ERROR;
                    response.message = "Error al actualizar stock: " + ex.Message;
                    response.data = null;
                }
            }

            return response;
        }


    }
}
