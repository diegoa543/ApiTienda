using System;
using System.Collections.Generic;

namespace Pruba_tienda_api.Models;

public partial class Perfile
{
    public int Id { get; set; }

    public string NombrePerfil { get; set; } = null!;

    public virtual ICollection<UsuariosPerfile> UsuariosPerfiles { get; set; } = new List<UsuariosPerfile>();
}
