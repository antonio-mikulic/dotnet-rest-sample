using System.Collections.Generic;
using RestSample.Data.Models.Products;

namespace RestSample.App.Dtos.Products
{
    public class ProductOutput
    {
        public ProductOutput()
        {
            Products = new List<Product>();
        }

        public List<Product> Products { get; set; }

        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public string[] AllSizes { get; set; }

        public string[] CommonWords { get; set; }
    }
}
