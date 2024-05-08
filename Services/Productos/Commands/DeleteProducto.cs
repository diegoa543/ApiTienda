using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;
using static Pruba_tienda_api.Services.Usuarios.Queries.GetUsuario;

namespace Pruba_tienda_api.Services.Productos.Commands
{
    public class DeleteProducto
    {
        public class DeleteProductoCommand : IRequest<Producto>
        {
            public int Id { get; set; }
        }

        public class DeleteProductoCommandValidation : AbstractValidator<DeleteProductoCommand>
        {
            public DeleteProductoCommandValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class DeleteProductoHandler : IRequestHandler<DeleteProductoCommand, Producto>
        {
            private readonly ApplicationContext _context;
            private readonly DeleteProductoCommandValidation _validation;

            public DeleteProductoHandler(ApplicationContext context, DeleteProductoCommandValidation validation)
            {
                _context = context;
                _validation = validation;
            }

            public async Task<Producto> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == request.Id);
                    if (producto != null)
                    {
                        producto.Estado = 0;
                        await _context.SaveChangesAsync(cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);

                        return producto;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("El producto que desea actualizar no existe.");
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
