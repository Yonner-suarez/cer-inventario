using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace microInventario.API.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventarioController: ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public ActionResult Producto(int idEmpleado)
        {
            //GeneralResponse res = BLActivacionContrato.ListarEstados(idEmpleado);
            return Ok();
        }
    }
}
