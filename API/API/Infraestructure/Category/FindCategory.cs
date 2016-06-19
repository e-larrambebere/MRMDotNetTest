using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using MediatR;

namespace API.Infraestructure.Category
{
    public class FindCategory
    {
        public class Query : IAsyncRequest<Model>
        {
            public int Id { get; private set; }

            public Query(int id)
            {
                this.Id = id;
            }
        }

        public class Model
        {
            public int CategoryID { get; set; }

            public string Name { get; set; }
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
                var modelFromDatabase = context.Categories
                    .SingleOrDefaultAsync(c => c.ID == message.Id);

                if (modelFromDatabase.Result != null)
                {
                    model = new Model
                    {
                        CategoryID = modelFromDatabase.Result.ID,
                        Name = modelFromDatabase.Result.Name
                    };
                }

                return model;
            }
        }
    }
}