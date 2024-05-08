using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;
using static Pruba_tienda_api.Services.Usuarios.Queries.GetUsuario;

namespace Pruba_tienda_api.Services.Categorias.Queries
{
    public class GetCategoria
    {
        public class GetCategoriaQuery : IRequest<Categoria>
        {
            public int? Id { get; set; }
        }

        public class GetCategoriaQueryValidation : AbstractValidator<GetCategoriaQuery>
        {
            public GetCategoriaQueryValidation()
            {
                RuleFor(x => x.Id).Cascade(CascadeMode.Stop).NotEmpty();
            }
        }

        public class GetUsuarioHandler : IRequestHandler<GetCategoriaQuery, Categoria>
        {
            private ApplicationContext _context;
            private GetCategoriaQueryValidation _validation;

            public GetUsuarioHandler(ApplicationContext context, GetCategoriaQueryValidation validation)
            {
                _context = context;
                _validation = validation;
            }

            public async Task<Categoria> Handle(GetCategoriaQuery request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var categoria = await _context.Categorias.FirstOrDefaultAsync(x => x.Id == request.Id);
                    if (categoria != null)
                    {
                        return categoria;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(categoria));
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
