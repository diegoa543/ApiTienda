using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pruba_tienda_api.Models;
using Pruba_tienda_api.Services.Categorias.Queries;
using static Pruba_tienda_api.Services.Categorias.Commands.InsertarCategoria;
using static Pruba_tienda_api.Services.Categorias.Queries.GetCategoria;

namespace Pruba_tienda_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        [Route("getCategoria")]
        public async Task<Categoria> GetCategoriaById(int id) =>
            await _mediator.Send(new GetCategoriaQuery { Id = id });

        [HttpPost]
        [Authorize]
        [Route("saveCategoria")]
        public async Task<Categoria> InsertarCategoria([FromBody] InsertarCategoriaCommand insertarCategoriaCommand) =>
            await _mediator.Send(insertarCategoriaCommand);
    }
}
