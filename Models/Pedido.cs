using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pruba_tienda_api.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Usuario? Cliente { get; set; }

    [JsonIgnore]
    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();
}
