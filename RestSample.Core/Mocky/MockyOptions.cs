namespace RestSample.Core.Mocky
{
    public class MockyOptions
    {
        // Options provided by mocky
        // List of api keys could also be used instead of primary and secondary api key

        public string BaseUrl { get; set; }
        public string ProductsPage { get; set; }
        public string PrimaryApiKey { get; set; }
        public string SecondaryApiKey { get; set; }
        public int DefaultCacheDuration { get; set; }
    }
}
