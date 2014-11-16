using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiSample.Models;

namespace WebApiSample.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        public static readonly IProductRepository ProductRepository = new ProductRepository();

        //Want a link to a tutorial on RESTful WebAPI 2.2: 
        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/create-a-rest-api-with-attribute-routing

        //Also can reach actions similar to this approach without WebApiConfig changes and RouteAttribute:
        //http://localhost:49487/api/Products?action=GetAllProducts

        //For StructureMap incase using a real DB and a real DataLayer
        //Install-Package StructureMap.WebApi2

        /*GET   api/products
                api/products?id=1
                api/products?category=Sports 
                api/products?departmentId=1         
                api/products?category=Sports&departmentId=2
        */
        [HttpGet]
        [Route("")]
        public IHttpActionResult RootRoute(int? id = null, string category = null, int? departmentId = null)
        {
            var products = new List<Product>();

            if (id != null)
            {
                var productFound = ProductRepository.Get(Convert.ToInt32(id));
                products.Add(productFound);
            }

            if (category != null)
            {
                products.AddRange(ProductRepository.GetAll().Where(product =>
                string.Equals(product.Category, HttpUtility.HtmlDecode(category), StringComparison.OrdinalIgnoreCase)));
            }

            if (departmentId != null)
            {
                products.AddRange(ProductRepository.GetAll().Where(product => product.DepartmentId == departmentId));
            }

            if (id == null && category == null && departmentId == null)
            {
                products.AddRange(ProductRepository.GetAll().ToList());
            }

            return Ok(products);
        }

        // GET api/products/GetAllProducts
        [HttpGet]
        [Route("GetAllProducts")]
        public IEnumerable<Product> GetAllProducts()
        {
            return ProductRepository.GetAll() as List<Product>;
        }

        // GET api/products/GetAllProductsWithExtra
        [HttpGet]
        [Route("GetAllProductsWithExtra")]
        public IEnumerable<Product> GetAllProductsWithExtra()
        {
            var randomProdIds = new List<Random> { new Random(), new Random(), new Random() };

            var productsList = ProductRepository.GetAll() as List<Product> ?? new List<Product>();

            var extraProducts = new List<Product>()
            {
              new Product {Id = randomProdIds[0].Next(20, 30), Name = "1stNewProduct", Category = "Gaming", Price = 11.00M, DepartmentId = 4 },
              new Product {Id = randomProdIds[1].Next(40, 50), Name = "2ndNewProduct", Category = "Gaming", Price = 12.00M, DepartmentId = 4 },
              new Product {Id = randomProdIds[2].Next(60, 70), Name = "3rdNewProduct", Category = "Gaming", Price = 13.00M, DepartmentId = 4 }
            };

            randomProdIds = null;

            productsList.AddRange(extraProducts);

            return productsList;
        }

        // GET api/products/categories/Gaming
        [Route("categories/{category}")]
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return ProductRepository.GetAll().Where(product =>
                string.Equals(product.Category, HttpUtility.HtmlDecode(category), StringComparison.OrdinalIgnoreCase));
        }

        // GET api/products/5
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            Product product;

            try
            {
                product = ProductRepository.Get(id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //GET api/products/GetProductById/1
        [HttpGet]
        [Route("GetProductById/{id:int}", Order = 1)]
        public IHttpActionResult GetProductById(int id)
        {
            if (id <= 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            Product product;

            try
            {
                product = ProductRepository.Get(id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //GET api/products/GetProductByIdTwo/1
        [HttpGet]
        [Route("GetProductByIdTwo/{id:int}", Order = 2)]
        public IHttpActionResult GetProductByIdTwo(int id)
        {
            if (id <= 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            Product product;

            try
            {
                product = ProductRepository.Get(id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //GET api/products/GetProductByIdThree/1
        [HttpGet]
        [Route("GetProductByIdThree/{id:int}", Order = 3)]
        public IHttpActionResult GetProductByIdThree(int id)
        {
            if (id <= 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            Product product;

            try
            {
                product = ProductRepository.Get(id);
            }
            catch (Exception ex)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST api/product/5
        [HttpPost]
        public HttpResponseMessage PostProduct(Product product)
        {
            if (product == null) return Request.CreateResponse(HttpStatusCode.BadRequest);
            product = ProductRepository.Add(product);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, product);
            response.Headers.Location = new Uri(Url.Link("Default", new { id = product.Id }));
            return response;
        }

        //PUT (UPDATE) api/product/5
        [HttpPut]
        public HttpResponseMessage PutProduct(int id, Product product)
        {
            if (product.Id != id) return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                ProductRepository.Update(product);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/products/5
        [HttpDelete]
        public HttpResponseMessage Delete(Product product)
        {
            if (product == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

            try
            {
                ProductRepository.Remove(product.Id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
