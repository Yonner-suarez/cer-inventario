using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microInventario.API.Util.ActionResults
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
            //Value = error.ToString();
        }
    }
}
