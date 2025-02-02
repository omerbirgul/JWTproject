using System.Net;
using AuthServer.Core.Dtos.ResponseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateCustomResult<T>(ResponseDto<T> responseDto) where T : class
        {
            return responseDto.StatusCode switch
            {
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.Created => Created(),
                _ => new ObjectResult(responseDto) { StatusCode = responseDto.StatusCode.GetHashCode() }
            };
        }
    }
}
