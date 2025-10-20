using microInventario.API.BL;
using microInventario.API.Model;
using microInventario.API.Model.Request;
using microInventario.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace microInventario.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventarioController: ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public ActionResult Producto(AgregarProductoRequest producto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return StatusCode(Variables.Response.Inautorizado, null);


            var claims = identity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return StatusCode(Variables.Response.BadRequest, new GeneralResponse
                {
                    data = null,
                    status = Variables.Response.BadRequest,
                    message = "Solo los Administradores pueden crear productos"
                });
            }
            var idAdmin = int.Parse(claims.FirstOrDefault(c => c.Type == "idUser")?.Value);


            GeneralResponse res = BLInventario.AgregarProducto(producto, idAdmin);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }

        [HttpPut]
        [Route("[action]/{IdProducto}/{nuevaCantidad}")]
        public ActionResult CantidadProducto([Required]int IdProducto, [Required]int nuevaCantidad)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return StatusCode(Variables.Response.Inautorizado, null);


            var claims = identity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return StatusCode(Variables.Response.BadRequest, new GeneralResponse
                {
                    data = null,
                    status = Variables.Response.BadRequest,
                    message = "Solo los Administradores pueden crear productos"
                });
            }

            var idAdmin = int.Parse(claims.FirstOrDefault(c => c.Type == "idUser")?.Value);


            GeneralResponse res = BLInventario.ActualizarCantidad(IdProducto,nuevaCantidad, idAdmin);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }


        [HttpDelete]
        [Route("[action]/{IdProducto}")]
        public ActionResult Producto([Required] int IdProducto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return StatusCode(Variables.Response.Inautorizado, null);


            var claims = identity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return StatusCode(Variables.Response.BadRequest, new GeneralResponse
                {
                    data = null,
                    status = Variables.Response.BadRequest,
                    message = "Solo los Administradores pueden crear productos"
                });
            }

            var idAdmin = int.Parse(claims.FirstOrDefault(c => c.Type == "idUser")?.Value);


            GeneralResponse res = BLInventario.EliminarProducto(IdProducto, idAdmin);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }

        [HttpPut]
        [Route("[action]/{idProducto}")]
        public ActionResult ProductoData(ActualizarInfoProductoRequest request, [Required] int idProducto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return StatusCode(Variables.Response.Inautorizado, null);


            var claims = identity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return StatusCode(Variables.Response.BadRequest, new GeneralResponse
                {
                    data = null,
                    status = Variables.Response.BadRequest,
                    message = "Solo los Administradores pueden crear productos"
                });
            }

            var idAdmin = int.Parse(claims.FirstOrDefault(c => c.Type == "idUser")?.Value);


            GeneralResponse res = BLInventario.ActualizarProductoData(request, idProducto,idAdmin);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }

        [HttpGet]
        [Route("[action]/{idProducto}")]
        public ActionResult ProductoById([Required] int idProducto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return StatusCode(Variables.Response.Inautorizado, null);


            var claims = identity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role != "Administrador")
            {
                return StatusCode(Variables.Response.BadRequest, new GeneralResponse
                {
                    data = null,
                    status = Variables.Response.BadRequest,
                    message = "Solo los Administradores pueden consultar productos"
                });
            }


            GeneralResponse res = BLInventario.ObtenerProducto(idProducto);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }
        [HttpGet]
        [Route("[action]")]
        public ActionResult Productos()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null) return StatusCode(Variables.Response.Inautorizado, null);


            var claims = identity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == "Cliente")
            {
                return StatusCode(Variables.Response.BadRequest, new GeneralResponse
                {
                    data = null,
                    status = Variables.Response.BadRequest,
                    message = "Solo los Administradores o Logistica pueden consultar productos"
                });
            }


            GeneralResponse res = BLInventario.ObtenerProductos();
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }

        //porque se ejecuta cuando wompi dispara evento de pago success
        [AllowAnonymous]
        [HttpPut]
        [Route("[action]")]
        public ActionResult ActualizarStock([FromBody] List<ActualizarStockProducto> request)
        {
            GeneralResponse res = BLInventario.ActualizarStockCuandoPagoConfirmnado(request);
            if (res.status == Variables.Response.OK)
            {
                return Ok(res);
            }
            else
            {
                return StatusCode(res.status, res);
            }
        }
    }
}
