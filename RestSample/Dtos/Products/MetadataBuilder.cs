namespace RestSample.App.Dtos.Products
{
    public class MetadataBuilder
    {
        public bool IncludeProducts { get; set; }
        public int SkipMostCommonWords { get; set; }
        public int TakeNextMostCommonWords { get; set; }
    }
}
