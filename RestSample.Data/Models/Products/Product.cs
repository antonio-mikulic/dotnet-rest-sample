using System.Collections.Generic;

namespace RestSample.Data.Models.Products
{
    public class Product
    {
        public Product()
        {
            Sizes = new List<string>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public List<string> Sizes { get; set; }

        public string Description { get; set; }
    }
}
