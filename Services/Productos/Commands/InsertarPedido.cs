using FluentValidation;
using MailKit.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Pruba_tienda_api.Data;
using Pruba_tienda_api.Models;
using System.Net;
using System.Net.Mail;
using System.Security.AccessControl;
using static Pruba_tienda_api.Services.Productos.Commands.InsertarProductos;


namespace Pruba_tienda_api.Services.Productos.Commands
{
    public class InsertarPedido
    {
        public class InsertarPedidoCommand : IRequest<Pedido> 
        {
            public int ClienteId { get; set; }
            public List<DetallesPedido> Detalles { get; set; }
        }

        public class InsertarDetallePedidoCommand 
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
        }


        public class InsertarPedidoCommandValidation : AbstractValidator<InsertarPedidoCommand>
        {
            public InsertarPedidoCommandValidation()
            {
                RuleFor(x => x.ClienteId).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Detalles).Cascade(CascadeMode.Stop).NotEmpty();
            }
        }

        public class InsertarDetallePedidoCommandValidation : AbstractValidator<InsertarDetallePedidoCommand>
        {
            public InsertarDetallePedidoCommandValidation()
            {
                RuleFor(x => x.ProductoId).Cascade(CascadeMode.Stop).NotEmpty();
                RuleFor(x => x.Cantidad).Cascade(CascadeMode.Stop).NotEmpty();

            }
        }

        public class InsertarPedidoCommandHandler : IRequestHandler<InsertarPedidoCommand, Pedido>
        {
            private readonly ApplicationContext _context;
            private readonly InsertarPedidoCommandValidation _validation;

            public InsertarPedidoCommandHandler(ApplicationContext dbContext, InsertarPedidoCommandValidation validation)
            {
                _context = dbContext;
                _validation = validation;
            }

            public async Task<Pedido> Handle(InsertarPedidoCommand request, CancellationToken cancellationToken)
            {
                _validation.Validate(request);
                try
                {
                    var pedido = new Pedido
                    {
                        ClienteId = request.ClienteId,
                        Fecha = DateTime.Now,
                        Total = 0, //Iniciamos el total con 0
                        Estado = "Activo"
                    };                    
                    await _context.Pedidos.AddAsync(pedido);
                    await _context.SaveChangesAsync(cancellationToken);

                    foreach (var detalle in request.Detalles)
                    {
                        var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == detalle.ProductoId);
                        if (producto == null || producto.Cantidad < detalle.Cantidad)
                            throw new Exception("Producto no disponible o cantidad insuficiente");

                        producto.Cantidad -= detalle.Cantidad;

                        var detallePedido = new DetallesPedido
                        {
                            PedidoId = pedido.Id,
                            ProductoId = detalle.ProductoId,
                            Cantidad = detalle.Cantidad,
                            Precio = producto.Precio * detalle.Cantidad
                        };

                        pedido.Total += detallePedido.Precio;

                        _context.DetallesPedidos.AddAsync(detallePedido);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    await EnviarCorreo(pedido);

                    return pedido;
                }
                catch (Exception ex)
                {

                    throw ex;
                }                
            }

            private async Task EnviarCorreo(Pedido pedido)
            {
                var cliente = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == pedido.ClienteId);
                var detalles = await _context.DetallesPedidos.Where(x => x.PedidoId == pedido.Id).ToListAsync();
                //var mensaje = new MailMessage();
                var email = new MimeMessage();

                if (cliente != null && detalles != null)
                {
                    email.From.Add(new MailboxAddress("Diego", "diegogonzalez6956@gmail.com"));
                    email.To.Add(new MailboxAddress(cliente.Nombre, cliente.Email));
                    email.Subject = "Confirmación de pedido";

                    email.Body = new TextPart("plain")
                    {
                        Text = $"Estimado/a {cliente.Nombre},\n" +
                        $"¡Gracias por tu compra!\n" +
                        $"Este correo es para confirmar que hemos recibido tu pedido correctamente. A continuación, encontrarás los detalles de tu pedido:\n" +
                        string.Join("\n", detalles.Select(x => $"- Producto: {x.Producto.Nombre}, Cantidad: {x.Cantidad}, Precio: {x.Precio}")) +
                        $"\n\nTotal: {pedido.Total}\n\n" +
                        $"Si necesitas realizar algún cambio en tu pedido o tienes alguna pregunta, no dudes en ponerte en contacto con nuestro equipo de atención al cliente.\r\n" +
                        $"Gracias por confiar en nosotros.\n" +
                        $"Atentamente,\n" +
                        $"Diego Gonzalez\n" +
                        $"Gerente del establecimiento."
                    };

                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync("pruebaapi59@gmail.com", "qckx ntxu asay yydy");
                        await client.SendAsync(email);
                        await client.DisconnectAsync(true);
                    }

                    //mensaje.From = new MailAddress("diegogonzalez6956@gmail.com");
                    //mensaje.To.Add(new MailAddress(cliente.Email));
                    //mensaje.Subject = "Confirmación de pedido";
                    //mensaje.Body = $"Estimado/a {cliente.Email},\n" +
                    //    $"¡Gracias por tu compra!\n" +
                    //    $"Este correo es para confirmar que hemos recibido tu pedido correctamente. A continuación, encontrarás los detalles de tu pedido:";
                    //foreach (var detalle in detalles)
                    //{
                    //    var producto = await _context.Productos.FirstOrDefaultAsync(x => x.Id == detalle.ProductoId);
                    //    if (producto != null)
                    //    {
                    //        mensaje.Body += $"- Producto: {producto.Nombre}\n" +
                    //        $"- Cantidad: {detalle.Cantidad}, \n" +
                    //        $"- Precio: {detalle.Precio}\n";
                    //    }
                    //    else
                    //    {
                    //        throw new UnauthorizedAccessException("No existe el producto.");
                    //    }
                    //}
                    //mensaje.Body += $"\nTotal: {pedido.Total}\n" +
                    //    $"Si necesitas realizar algún cambio en tu pedido o tienes alguna pregunta, no dudes en ponerte en contacto con nuestro equipo de atención al cliente.\r\n" +
                    //    $"Gracias por confiar en nosotros.\n" +
                    //    $"Atentamente,\n" +
                    //    $"Diego Gonzalez\n" +
                    //    $"Gerente del establecimiento.";

                    //using (var smtp = new SmtpClient("smtp.ejemplo.com", 587)) 
                    //{
                    //    smtp.Credentials = new NetworkCredential("pruebaapi59@gmail.com", "qckx ntxu asay yydy");
                    //    smtp.EnableSsl = true;
                    //    await smtp.SendMailAsync(mensaje);

                    //}
                }
            }
        }
    }
}

