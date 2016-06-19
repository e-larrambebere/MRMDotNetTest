using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using MediatR;

namespace API.Infraestructure.Product
{
    public class FindProduct
    {
        public class Query : IAsyncRequest<Model>
        {
            public int Id { get; private set; }

            public Query(int id)
            {
                Id = id;
            }
        }

        public class Model
        {
            public int ProductId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Model>
        {
            private readonly ApiContext context;

            public QueryHandler(ApiContext context)
            {
                this.context = context;
            }

            public async Task<Model> Handle(Query message)
            {
                Model model = null;
                var modelFromDatabase = context.Products
                    .SingleOrDefaultAsync(product => product.ID == message.Id);

                if (modelFromDatabase.Result != null)
                {
                    model = new Model
                    {
                        ProductId = modelFromDatabase.Result.ID,
                        Name = modelFromDatabase.Result.Name,
                        Description = modelFromDatabase.Result.Description
                    };
                }

                return model;
            }
        }
    }
}