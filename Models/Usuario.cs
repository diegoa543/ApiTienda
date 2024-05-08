using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pruba_tienda_api.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    [JsonIgnore]
    public virtual ICollection<UsuariosPerfile> UsuariosPerfiles { get; set; } = new List<UsuariosPerfile>();
}
