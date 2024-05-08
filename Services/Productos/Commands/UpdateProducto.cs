using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;
using static Pruba_tienda_api.Business.UsuarioBusiness.Commands.SaveUsuario;

namespace Pruba_tienda_api.Services.Productos.Commands
{
    public class UpdateProductoCommand : IRequest<Producto>
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }

        public string? Descripción { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

    }

    public class UpdateProductoCommandValidation : AbstractValidator<UpdateProductoCommand>
    {
        public UpdateProductoCommandValidation()
        {
            RuleFor(x => x.Id).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(x => x.Nombre).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(x => x.Descripción).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(x => x.Precio).Cascade(CascadeMode.Stop).NotEmpty();
            RuleFor(x => x.Cantidad).Cascade(CascadeMode.Stop).NotEmpty();

        }
    }

    public class UpdateProductoHandler : IRequestHandler<UpdateProductoCommand, Producto>
    {
        private readonly ApplicationContext _context;
        private readonly UpdateProductoCommandValidation _validation;

        public UpdateProductoHandler(ApplicationContext context, UpdateProductoCommandValidation validation)
        {
            _context = context;
            _validation = validation;
        }

        public async Task<Producto> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
        {
            _validation.Validate(request);
            try
            {
                var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (producto != null)
                {
                    producto.Nombre = request.Nombre;
                    producto.Descripción = request.Descripción;
                    producto.Precio = request.Precio;
                    producto.Cantidad = request.Cantidad;
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
