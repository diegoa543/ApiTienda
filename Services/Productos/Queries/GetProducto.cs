using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.DTO;
using Pruba_tienda_api.Models;
using static Pruba_tienda_api.Services.Categorias.Queries.GetCategoria;

namespace Pruba_tienda_api.Services.Productos.Queries
{
    public class GetProducto
    {
        public class GetProductoQuery : IRequest<List<ProductoDTO>>
        {
            public int Estado { get; set; }
        }

        public class GetProductoQueryValidation : AbstractValidator<GetProductoQuery>
        {
            public GetProductoQueryValidation()
            {
                RuleFor(x => x.Estado).Cascade(CascadeMode.Stop).NotEmpty();
            }
        }

        public class GetProductoHandle : IRequestHandler<GetProductoQuery, List<ProductoDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly GetProductoQueryValidation _validation;

            public GetProductoHandle(ApplicationContext context, GetProductoQueryValidation validation)
            {
                _context = context;
                _validation = validation;
            }
            public async Task<List<ProductoDTO>> Handle(GetProductoQuery request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var producto = await _context.Productos
                        .Where(x => x.Estado == request.Estado)
                        .Select(x => new ProductoDTO { Nombre = x.Nombre, Descripcion = x.Descripción, Precio = x.Precio, Cantidad = x.Cantidad})
                        .ToListAsync();
                    return producto;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
    }
}
