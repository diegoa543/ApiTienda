using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pruba_tienda_api.DTO;
using Pruba_tienda_api.Models;
using Pruba_tienda_api.Services.Productos.Commands;
using static Pruba_tienda_api.Business.UsuarioBusiness.Commands.SaveUsuario;
using static Pruba_tienda_api.Services.Productos.Commands.DeleteProducto;
using static Pruba_tienda_api.Services.Productos.Commands.InsertarProductos;
using static Pruba_tienda_api.Services.Productos.Queries.GetProducto;
using static Pruba_tienda_api.Services.Productos.Queries.GetProductoAdm;

namespace Pruba_tienda_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Get")]
        [Authorize]
        public async Task<ActionResult<List<ProductoDTO>>> GetProductos() =>
            await _mediator.Send(new GetProductoQuery { Estado = 1 });

        [HttpGet]
        [Route("GetAdm")]
        [Authorize]
        public async Task<ActionResult<List<Producto>>> GetProductosAdm()
        {
            var productoAdm = await _mediator.Send(new GetProductoAdmQuery { Estado = 1 });
            return Ok(productoAdm);
        }            

        [HttpPost]
        [Route("save")]
        [Authorize]
        public async Task<Producto> InsertProducto([FromBody] InsertProductoCommand insertProductoCommand) =>
            await _mediator.Send(insertProductoCommand);

        [HttpPut]
        [Route("Update")]
        [Authorize]
        public async Task<Producto> UpdateProducto([FromBody] UpdateProductoCommand updateProductoCommand) =>
            await _mediator.Send(updateProductoCommand);

        [HttpDelete]
        [Route("Delete")]
        [Authorize]
        public async Task<Producto> DeleteProducto(int id) =>
            await _mediator.Send(new DeleteProductoCommand { Id= id });
    }
}
