using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.DTO;
using Pruba_tienda_api.Models;
using static Pruba_tienda_api.Services.Productos.Queries.GetProducto;

namespace Pruba_tienda_api.Services.Productos.Queries
{
    public class GetProductoAdm
    {
        public class GetProductoAdmQuery : IRequest<List<ProductoAdmDTO>>
        {
            public int Estado { get; set; }
        }

        public class GetProductoAdmQueryValidation : AbstractValidator<GetProductoAdmQuery>
        {
            public GetProductoAdmQueryValidation()
            {
                RuleFor(x => x.Estado).Cascade(CascadeMode.Stop).NotEmpty();
            }
        }

        public class GetProducAdmtoHandle : IRequestHandler<GetProductoAdmQuery, List<ProductoAdmDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly GetProductoAdmQueryValidation _validation;

            public GetProducAdmtoHandle(ApplicationContext context, GetProductoAdmQueryValidation validation)
            {
                _context = context;
                _validation = validation;
            }
            public async Task<List<ProductoAdmDTO>> Handle(GetProductoAdmQuery request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var producto = await _context.Productos
                        .Where(x => x.Estado == request.Estado)
                        .Include(x => x.Categoria)
                        .Select(x => new ProductoAdmDTO { ProductoId = x.Id, 
                            NombreProducto = x.Nombre, 
                            Descripcion = x.Descripción,
                            Precio = x.Precio,
                            Cantidad= x.Cantidad,
                            NombreCategoria = string.Join(", ", x.Categoria.Select(c => c.Nombre))
                        })
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
