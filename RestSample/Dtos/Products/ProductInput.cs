namespace RestSample.App.Dtos.Products
{
    public class ProductInput : IInputFilter
    {
        public ProductInput()
        {
            Size = new string[] { };
            Highlight = new string[] { };
        }

        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }

        public string[] Size { get; set; }
        public string[] Highlight { get; set; }
    }
}