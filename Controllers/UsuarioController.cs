using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pruba_tienda_api.Models;
using static Pruba_tienda_api.Business.UsuarioBusiness.Commands.SaveUsuario;
using static Pruba_tienda_api.Services.Usuarios.Queries.GetUsuario;

namespace Pruba_tienda_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator  _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("save")]
        [Authorize]
        public async Task<Usuario> SaveUsuario([FromBody] SaveUsuarioCommand saveUsuarioCommand) =>
            await _mediator.Send(saveUsuarioCommand);

        [HttpGet]
        [Route("get-by-id/{id}")]
        [Authorize]
        public async Task<Usuario> GetUsuarioById(int id) =>
            await _mediator.Send(new GetUsuarioQuery{ Id = id });

    }
}
