using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using MediatR;

namespace API.Infraestructure.Product
{
    public class ListProducts
    {
        public class Query : IAsyncRequest<IQueryable<Model>>
        {
        }

        public class Model
        {
            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public string ProductDescription { get; set; }

            public string CategoryName { get; set; }
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
                return context.Products.Include("category").Select(p => new Model
                {
                    ProductId = p.ID,
                    ProductName = p.Name,
                    ProductDescription = p.Description,
                    CategoryName = p.Category.Name
                });
            }
        }
    }
}