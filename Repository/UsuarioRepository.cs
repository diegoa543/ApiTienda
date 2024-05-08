using Pruba_tienda_api.Data;
using Pruba_tienda_api.Interface;

namespace Pruba_tienda_api.Repository
{
    public class UsuarioRepository : IUsuario
    {
        readonly ApplicationContext _dbContext = new();
    }
}
