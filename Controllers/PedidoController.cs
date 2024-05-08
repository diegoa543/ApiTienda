using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using static Pruba_tienda_api.Services.Productos.Commands.InsertarPedido;

namespace Pruba_tienda_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PedidoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        [Route("save")]
        public async Task<IActionResult> CrearPedido([FromBody] InsertarPedidoCommand insertarPedidoCommand)
        {
            var pedido = await _mediator.Send(insertarPedidoCommand);
            return Ok(pedido);
        }
    }
}
