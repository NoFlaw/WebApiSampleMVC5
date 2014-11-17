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

        /// <summary>
        /// Constructor injection of ProductService
        /// </summary>
        /// <param name="productService"></param>
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
        /// <summary>
        /// Endpoint provides querystring search abilities for ProductId, ProductCategory, and ProductDepartingId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult QueryStringSearch(int? id = null, string category = null, int? departmentId = null)
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
        /// <summary>
        /// Returns an IEnumerable List of Products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProducts")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _productService.GetAll();
        }

        // GET api/products/GetAllProductsWithExtra
        /// <summary>
        /// Returns an IEnumerable List of Products with 3 extra added products
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllProductsWithExtra")]
        public IEnumerable<Product> GetAllProductsWithExtra()
        {
            var products = _productService.AddExtraProducts();
            return products;
        }

        // GET api/products/categories/Gaming
        /// <summary>
        /// Returns an IEnumerable List of Products filtered by category provided 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("categories/{category}")]
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            var products = _productService.GetAll().Where(product =>
                string.Equals(product.Category, category, StringComparison.OrdinalIgnoreCase));

            return products;
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
        /// <summary>
        /// Returns a given product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns a given product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns a given product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Saves/Posts a given product 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage PostProduct(Product product)
        {
            if (product == null) return Request.CreateResponse(HttpStatusCode.BadRequest);

            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, _productService.Add(product));
            response.Headers.Location = new Uri(Url.Link("Default", new { id = product.Id }));
            return response;
        }

        //PUT (UPDATE) api/product/5
        /// <summary>
        /// HTTP PUT for a given product by id and product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
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
        /// <summary>
        /// HTTP DELETE for a given product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
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
