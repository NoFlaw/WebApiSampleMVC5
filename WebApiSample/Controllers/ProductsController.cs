using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApiSample.Models;
using WebApiSample.Services;

namespace WebApiSample.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /*GET   
         * Also can reach actions similar to this approach without WebApiConfig changes and RouteAttribute:
         * http://localhost:49487/api/Products?action=GetAllProducts
         * 
                api/products?category=Sports 
                api/products?departmentId=1         
                api/products?category=Sports&departmentId=2
                api/products?id=1&category=Sports&departmentId=2
        */

        [HttpGet]
        [Route("")]
        public IHttpActionResult RootRoute(int? id = null, string category = null, int? departmentId = null)
        {
            var products = new List<Product>();

            if (id != null)
            {
                var productFound = _productService.GetById(Convert.ToInt32(id));
                products.Add(productFound);
            }

            if (category != null)
            {
                products.AddRange(_productService.GetAll().Where(product =>
                string.Equals(product.Category, HttpUtility.HtmlDecode(category), StringComparison.OrdinalIgnoreCase)));
            }

            if (departmentId != null)
            {
                products.AddRange(_productService.GetAll().Where(product => product.DepartmentId == departmentId));
            }

            if (id == null && category == null && departmentId == null)
            {
                products.AddRange(_productService.GetAll().ToList());
            }

            return Ok(products);
        }

        // GET api/products/GetAllProducts
        [HttpGet]
        [Route("GetAllProducts")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _productService.GetAll();
        }

        // GET api/products/GetAllProductsWithExtra
        [HttpGet]
        [Route("GetAllProductsWithExtra")]
        public IEnumerable<Product> GetAllProductsWithExtra()
        {
            var products = _productService.AddExtraProducts();
            return products;
        }

        // GET api/products/categories/Gaming
        [Route("categories/{category}")]
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _productService.GetAll().Where(product =>
                string.Equals(product.Category, HttpUtility.HtmlDecode(category), StringComparison.OrdinalIgnoreCase));
        }

        // GET api/products/5
        /// <summary>
        /// Returns a given Product by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest));

            Product product;

            try
            {
                product = _productService.GetById(id);
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
                product = _productService.GetById(id);
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
                product = _productService.GetById(id);
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
                product = _productService.GetById(id);
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

            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, _productService.Add(product));
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
                _productService.Update(product);
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
                _productService.Remove(product);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
