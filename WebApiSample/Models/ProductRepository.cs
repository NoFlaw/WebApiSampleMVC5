using System;
using System.Collections.Generic;

namespace WebApiSample.Models
{
    public class ProductRepository : IProductRepository
    {
        private int _nextId = 1;
        private readonly List<Product> _products = new List<Product>();
        
        public ProductRepository()
        {
            Add(new Product { Name = "Raisin Bran", Category = "Food", Price = 1.50M, DepartmentId = 1});
            Add(new Product { Name = "G.I. JOE - Action Figure", Category = "Toys", Price = 3.75M, DepartmentId = 1 });
            Add(new Product { Name = "Hammer", Category = "Hardware", Price = 16.99M, DepartmentId = 2 });
            Add(new Product { Name = "Crayons", Category = "Utensils", Price = 0.99M, DepartmentId = 2 });
            Add(new Product { Name = "Golf Club", Category = "Sports", Price = 42.00M, DepartmentId = 3});
            Add(new Product { Name = "Basketball", Category = "Sports", Price = 15.54M, DepartmentId = 3 });
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product Get(int id)
        {
            return _products.Find(p => p.Id == id);
        }

        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            
            item.Id = _nextId++;
            _products.Add(item);
            return item;
        }

        public void Remove(int id)
        {
            _products.RemoveAll(p => p.Id == id);
        }

        public bool Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            
            var index = _products.FindIndex(p => p.Id == item.Id);
            
            if (index == -1)
            {
                return false;
            }
            _products.RemoveAt(index);
            _products.Add(item);
            return true;
        }
    }
}