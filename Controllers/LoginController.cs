using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pruba_tienda_api.DTO;
using Pruba_tienda_api.Services.Login.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pruba_tienda_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("iniciarSesion")]
        public async Task<TokenDto> IniciarSesion([FromBody]LoginQuery loginQuery) =>
            await _mediator.Send(loginQuery);
        ////public async Task<IActionResult> IniciarSesion ([FromBody]LoginQuery loginQuery)
        ////{
        ////    var token = await _mediator.Send(loginQuery);
        ////    return Ok(new { token });
        ////}
    }
}
