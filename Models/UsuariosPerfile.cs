using System;
using System.Collections.Generic;

namespace Pruba_tienda_api.Models;

public partial class UsuariosPerfile
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public int? PerfilId { get; set; }

    public virtual Perfile? Perfil { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
