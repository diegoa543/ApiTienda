using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;

namespace Pruba_tienda_api.Services.Usuarios.Queries
{
    public class GetUsuario
    {
        public class GetUsuarioQuery : IRequest<Usuario>
        {
            public int Id { get; set; }
        }
        public class GetUsuarioQueryValidation : AbstractValidator<GetUsuarioQuery>
        {
            public GetUsuarioQueryValidation()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class GetUsuarioHandler : IRequestHandler<GetUsuarioQuery, Usuario>
        {
            private ApplicationContext _context;
            private GetUsuarioQueryValidation _validation;

            public GetUsuarioHandler(ApplicationContext context, GetUsuarioQueryValidation validation)
            {
                _context = context;
                _validation = validation;
            }
    
            public async Task<Usuario> Handle(GetUsuarioQuery request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var usu = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id== request.Id);
                    if (usu != null) 
                    {
                        return usu;
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(usu));
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
