using System;
using System.Collections.Generic;
using System.Linq;
using WebApiSample.Models;

namespace WebApiSample.Services
{
    /// <summary>
    /// Interface for ProductSerice
    /// </summary>
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int id);
        Product Add(Product product);
        Product Update(Product product);
        bool Remove(Product product);
        IEnumerable<Product> AddExtraProducts();
    }

    /// <summary>
    /// Product Service exposing the Product Repository
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Returns ALL Products by a list
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            var productList = _productRepository.GetAll().ToList();
            return productList;
        }

        /// <summary>
        /// Returns a given Product by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetById(int id)
        {
            var productFound = _productRepository.Get(id);
            return productFound;
        }

        /// <summary>
        /// Returns added Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product Add(Product product)
        {
            if (String.IsNullOrEmpty(product.Name) || String.IsNullOrEmpty(product.Category))
                throw new Exception("Product Name nor Product Category can be null"); 
            _productRepository.Add(product);
            return product;
        }

        /// <summary>
        /// Update a given Product by parameter
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public Product Update(Product product)
        {
            var productUpdated = product;
            _productRepository.Update(productUpdated);
            return productUpdated;
        }

        /// <summary>
        /// Remove a given Product by parameter
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public bool Remove(Product product)
        {
            if (product.Id == 0) return false;
            _productRepository.Remove(product.Id);
            return true;
        }

        public IEnumerable<Product> AddExtraProducts()
        {
            var products = _productRepository.GetAll().ToList();
            var randomProdIds = new List<Random> { new Random(), new Random(), new Random() };
            
            var extraProducts = new List<Product>()
            {
              new Product {Id = randomProdIds[0].Next(20, 30), Name = "1stNewProduct", Category = "Gaming", Price = 11.00M, DepartmentId = 4 },
              new Product {Id = randomProdIds[1].Next(40, 50), Name = "2ndNewProduct", Category = "Gaming", Price = 12.00M, DepartmentId = 4 },
              new Product {Id = randomProdIds[2].Next(60, 70), Name = "3rdNewProduct", Category = "Gaming", Price = 13.00M, DepartmentId = 4 }
            };

            products.AddRange(extraProducts);
            randomProdIds = null;
            
            return products;
        }
    }
}