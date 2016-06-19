using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using MediatR;

namespace API.Infraestructure.Category
{
    public class FindCategoryProducts
    {
        public class Query : IAsyncRequest<IQueryable<Model>>
        {
            public int Id { get; private set; }

            public Query(int id)
            {
                this.Id = id;
            }
        }

        public class Model
        {
            public int ProductID { get; set; }

            public string Name { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, IQueryable<Model>>
        {
            private readonly ApiContext context;

            public QueryHandler(ApiContext context)
            {
                this.context = context;
            }

            public async Task<IQueryable<Model>> Handle(Query message)
            {
                return context.Products
                    .Where(p => p.Category.ID == message.Id)
                    .Select(p => new Model
                    {
                        Name  = p.Name,
                        ProductID = p.ID
                    });
            }
        }
    }
}