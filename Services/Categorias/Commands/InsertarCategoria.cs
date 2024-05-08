using FluentValidation;
using MediatR;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;

namespace Pruba_tienda_api.Services.Categorias.Commands
{
    public class InsertarCategoria
    {
        public class InsertarCategoriaCommand : IRequest<Categoria>
        {
            public string? Nombre { get; set; }
        }

        public class InsertarCategoriaCommandValidation : AbstractValidator<InsertarCategoriaCommand> 
        {
            public InsertarCategoriaCommandValidation()
            {
                RuleFor(x => x.Nombre).Cascade(CascadeMode.Stop).NotEmpty();
            }
        }

        public class InsertarCategoriaHandler : IRequestHandler<InsertarCategoriaCommand, Categoria>
        {
            private readonly ApplicationContext _context;
            private readonly InsertarCategoriaCommandValidation _validation;

            public InsertarCategoriaHandler(ApplicationContext context, InsertarCategoriaCommandValidation validation)
            {
                _context = context;
                _validation = validation;
            }

            public async Task<Categoria> Handle(InsertarCategoriaCommand request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    Categoria categoria = new Categoria { Nombre = request.Nombre };
                    await _context.Categorias.AddAsync(categoria);
                    await _context.SaveChangesAsync(cancellationToken);
                    return categoria;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
