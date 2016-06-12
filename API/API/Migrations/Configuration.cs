namespace API.Migrations
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    internal sealed class Configuration : DbMigrationsConfiguration<API.Models.ApiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            if (System.Diagnostics.Debugger.IsAttached == false)
                System.Diagnostics.Debugger.Launch();
        }

        protected override void Seed(Models.ApiContext context)
        {
            GetProducts(context);
            context.SaveChanges();
        }


        private void GetProducts(Models.ApiContext context)
        {
            // var names = assembly.GetManifestResourceNames();
            var resourceName = "API.products.json";
            var jsonData = GetEmbeddedResourceAsString(resourceName);

            var productsSeed = JsonConvert.DeserializeObject<ICollection<ProductSeed>>(jsonData);

            var categories = productsSeed
                .GroupBy(p => p.Category)
                .Select(c => new Category
                {
                    Name = c.Key,
                    Products = new List<Product>()
                });

            context.Categories.AddOrUpdate(categories.ToArray());
            context.SaveChanges();

            foreach (var productToSeed in productsSeed)
            {
                var category = context.Categories.FirstOrDefault(c => c.Name == productToSeed.Category);
                context.Products.AddOrUpdate(new Product
                {
                    Name = productToSeed.Name,
                    Description = productToSeed.Description,
                    Category = category
                });
            }
        }


        private string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        private class ProductSeed
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public string Category { get; set; }

        }
    }
}
