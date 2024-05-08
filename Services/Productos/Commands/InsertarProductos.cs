using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;

namespace Pruba_tienda_api.Services.Productos.Commands
{
    public class InsertarProductos
    {
        public class InsertProductoCommand : IRequest<Producto>
        {
            public string? Nombre { get; set; }

            public string? Descripción { get; set; }

            public decimal Precio { get; set; }

            public int Cantidad { get; set; }

            public string? Categoria { get; set; }

            public int Estado { get; set; }
        }


        public class InsertProductoCommandValidation : AbstractValidator<InsertProductoCommand>
        {
            public InsertProductoCommandValidation()
            {
                RuleFor(x => x.Nombre).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Descripción).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Precio).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Cantidad).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Categoria).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Estado).Cascade(CascadeMode.Stop).NotEmpty();

            }
        }

        public class InsertProductoHandler : IRequestHandler<InsertProductoCommand, Producto>
        {
            private readonly ApplicationContext _context;
            private readonly InsertProductoCommandValidation _validation;
            public InsertProductoHandler(ApplicationContext context, InsertProductoCommandValidation validation)
            {
                _context = context;
                _validation = validation;
            }

            public async Task<Producto> Handle(InsertProductoCommand request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Nombre == request.Categoria);
                    if (categoria != null)
                    {
                        Producto prod = new Producto
                        {
                            Nombre = request.Nombre,
                            Descripción = request.Descripción,
                            Precio = request.Precio,
                            Cantidad = request.Cantidad,
                            Categoria = new List<Categoria> { categoria },
                            Estado = request.Estado
                        };
                        await _context.Productos.AddAsync(prod);
                        await _context.SaveChangesAsync(cancellationToken);
                        return prod;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("No existe la categoria.");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
