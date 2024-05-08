using FluentValidation;
using MediatR;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;

namespace Pruba_tienda_api.Business.UsuarioBusiness.Commands
{
    public class SaveUsuario
    {
        public class SaveUsuarioCommand : IRequest<Usuario>
        {
            public string Nombre { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string Contraseña { get; set; } = null!;
        }
        public class SaveUsuarioCommandValidation : AbstractValidator<SaveUsuarioCommand>
        {
            public SaveUsuarioCommandValidation()
            {
                RuleFor(x => x.Nombre).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Email).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Contraseña).Cascade(CascadeMode.Stop).NotEmpty();

            }
        }

        public class SaveUsuarioHandler : IRequestHandler<SaveUsuarioCommand, Usuario>
        {
            private readonly ApplicationContext _context;
            private readonly SaveUsuarioCommandValidation _validation;
            public SaveUsuarioHandler(ApplicationContext context, SaveUsuarioCommandValidation validation)
            {
                _context = context;
                _validation = validation;
            }

            public async Task<Usuario> Handle(SaveUsuarioCommand request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    Usuario usu = new Usuario();
                    usu.Nombre = request.Nombre;
                    usu.Email = request.Email;
                    usu.Contraseña = request.Contraseña;
                    await _context.Usuarios.AddAsync(usu);
                    await _context.SaveChangesAsync(cancellationToken);
                    return usu;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
