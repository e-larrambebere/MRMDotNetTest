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
using API.Infraestructure.Product;
using API.Models;
using MediatR;

namespace API.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly ApiContext db;
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        // GET: api/Products
        public async Task<IQueryable<ListProducts.Model>> GetProducts()
        {
            var products = await mediator.SendAsync(new ListProducts.Query());

            return products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(FindProduct.Model))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            var model = await mediator.SendAsync(new FindProduct.Query(id));
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(CreateProduct.Command product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await mediator.SendAsync(product);

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            var model = await mediator.SendAsync(new FindProduct.Query(id));

            if (model == null)
            {
                return NotFound();
            }

            await mediator.SendAsync(new DeleteProduct.Command(id));

            return Ok(model);
        }
    }
}