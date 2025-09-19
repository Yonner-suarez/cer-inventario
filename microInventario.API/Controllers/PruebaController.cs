using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace microInventario.API.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PruebaController:ControllerBase
    {
        [HttpPost]
        [Route("[action]")]
        public ActionResult prueba()
        {
            //GeneralResponse res = BLActivacionContrato.ListarEstados(idEmpleado);
            return Ok();
        }
    }
}
