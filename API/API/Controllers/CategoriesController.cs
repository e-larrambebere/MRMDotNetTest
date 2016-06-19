using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using API.Infraestructure.Category;
using API.Models;
using MediatR;

namespace API.Controllers
{
    public class CategoriesController : ApiController
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/Categories
        public async Task<IQueryable<ListCategories.Model>> GetCategories([FromUri] int[] categoryIds)
        {
            var categories = await mediator.SendAsync(new ListCategories.Query(categoryIds ?? new int[0]));

            return categories;
        }

        // GET: api/Categories/5/products
        [Route("api/categories/{id}/products")]
        [HttpGet]
        public async Task<IQueryable<FindCategoryProducts.Model>> GetCategoryProducts(int id)
        {
            var products = await mediator.SendAsync(new FindCategoryProducts.Query(id));

            return products;
        }

        // GET: api/Categories/5
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            var model = await mediator.SendAsync(new FindCategory.Query(id));
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}