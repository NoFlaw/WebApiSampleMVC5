using System.Collections.Generic;

namespace WebApiSample.Models
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product Get(int id);
        Product Add(Product product);
        void Remove(int id);
        bool Update(Product product);
    }
}