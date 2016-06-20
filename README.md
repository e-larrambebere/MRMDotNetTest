# MRM Brand .Net excercise

MRM deals with many companies and handles a lot of product data.

Your job will be to build a very simple product database API.

In this repository you will find a JSON file: `product.json` with some product data. Each product will have the following fields:

```json
{
  "name": "XXXXX",
  "description": "XXXX",
  "category": "XXXX"
}
```

You will need to perform the following tasks:

1. Create an ASP.Net application that uses the Entity Framework with code first migrations and a seed method that loads the products.
2. Create API endpoints that support the following operations:
  1. Retrieve all products
  2. Retrieve a list of categories
  3. Retrieve the products for a category
  4. Creation of a new product (and if necessary the category for that product)
  5. Deletion of a product

This is intended to be a creative exercise so the structure of the data stored in the database and the organisation of the APIs is left as a design decision for you.

You should be looking to demonstrate the following:

* an understanding of the entity framework
* code first migrations
* seeding the database
* API design
* application architecture and best practices
* unit testing

### Running this sample API

1. Restore the NuGet packages.
2. Open up the _Package Management Console_ and run the migrations by executing `Update-Database`. Please note that if you're executing the solution in _Debug_ mode you will be asked to attach a debugger to the execution of the `Seed` method, if you are not interested in debugging this method dismiss the popup by selecting `No`.
3. Execute the solution like any other `Web API` project, the available endpoints are:
 * **GET** _api/categories_ to retrieve all categories.
 * **GET** _api/categories/1_ to retrieve the category with Id 1.
 * **GET** _api/categories/1/products_ to retrieve all products from the category with Id 1.
 * **GET** _api/categories?categoryids=1&categoryids=2_ to retrieve a list of categories.
 * **GET** _api/products_ to retrieve all products.
 * **GET** _api/products/1_ to retrieve the product with Id 1.
 * **POST** _api/products_ to create a new Product.
 * **DELETE** _api/products/1_ to delete the product with Id 1.
