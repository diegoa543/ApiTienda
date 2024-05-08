using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pruba_tienda_api.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripción { get; set; }

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public int? Estado { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    [JsonIgnore]
    public virtual ICollection<Categoria> Categoria { get; set; } = new List<Categoria>();
}
