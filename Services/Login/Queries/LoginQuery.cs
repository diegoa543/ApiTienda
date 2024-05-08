using Azure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.DTO;
using Pruba_tienda_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

namespace Pruba_tienda_api.Services.Login.Queries
{
    public class LoginQuery : IRequest<TokenDto>
    {
        public string? Email { get; set; }
        public string? Contra { get; set; }

    }

    public class LoginQueryValidation : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidation()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Contra).NotEmpty();
        }
    }

    public class LoginHandler : IRequestHandler<LoginQuery, TokenDto>
    {
        private ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public LoginHandler(ApplicationContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x =>
                    x.Email == request.Email &&
                    x.Contraseña == request.Contra);

                if (usuario != null)
                {
                    var perfil = await _context.UsuariosPerfiles
                        .FirstOrDefaultAsync(x =>
                        x.UsuarioId == usuario.Id);
                    
                    if (perfil != null)
                    {
                        var tokenString = GenerateJSONWebToken(usuario, perfil);
                        TokenDto tk = new() { Token = tokenString };
                        return tk;
                    }
                    else
                    {
                        throw new UnauthorizedAccessException("Usuario no encontrado");
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException("Usuario no encontrado");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GenerateJSONWebToken(Usuario userInfo, UsuariosPerfile usuariosPerfile)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            IEnumerable<Claim> claims = new List<Claim> {
                new Claim("correo", userInfo.Email),
                new Claim("nombre", userInfo.Nombre),
                new Claim("perfil", usuariosPerfile.PerfilId.ToString()??string.Empty)

            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
