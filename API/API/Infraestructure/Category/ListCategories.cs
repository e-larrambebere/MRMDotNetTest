using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using API.Models;
using MediatR;

namespace API.Infraestructure.Category
{
    public class ListCategories
    {
        public class Query : IAsyncRequest<IQueryable<Model>>
        {
            public int[] CategoryIds { get; private set; }

            public Query(int[] categoryIds)
            {
                CategoryIds = categoryIds;
            }
        }

        public class Model
        {
            public int CategoryID { get; set; }

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
                IQueryable<Models.Category> categories = context.Categories;

                if (message.CategoryIds.Any())
                {
                    categories = categories.Where(c => message.CategoryIds.Contains(c.ID));
                }

                return categories.Select(c => new Model
                {
                    CategoryID = c.ID,
                    Name = c.Name
                });
            }
        }
    }
}