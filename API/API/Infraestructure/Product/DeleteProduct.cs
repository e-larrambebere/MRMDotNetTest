using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using MediatR;

namespace API.Infraestructure.Product
{
    public class DeleteProduct
    {
        public class Command : IAsyncRequest
        {
            public int Id { get; private set; }

            public Command(int id)
            {
                Id = id;
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ApiContext db;

            public CommandHandler(ApiContext db)
            {
                this.db = db;
            }

            protected override async Task HandleCore(Command message)
            {
                var product = await db.Products.FindAsync(message.Id);

                db.Products.Remove(product);

                await db.SaveChangesAsync();
            }
        }
    }
}