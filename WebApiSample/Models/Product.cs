﻿namespace WebApiSample.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}