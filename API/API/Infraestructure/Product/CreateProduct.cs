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
    public class CreateProduct
    {
        public class Command : IAsyncRequest
        {
            public int Id { get; set; }
            
            [Required]
            public string Name { get; set; }
            
            [Required]
            public string Description { get; set; }

            [Required]
            public string Category { get; set; }
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
                var category = db.Categories.FirstOrDefault(c => c.Name == message.Category);

                if (category == null)
                {
                    category = new Models.Category
                    {
                        Name = message.Category
                    };

                    db.Categories.Add(category);
                }

                var modelProduct = new Models.Product
                {
                    Category = category,
                    Description = message.Description,
                    Name = message.Name
                };

                db.Products.Add(modelProduct);

                await db.SaveChangesAsync();
            }
        }
    }
}