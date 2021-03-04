using System.Collections.Generic;

namespace RestSample.Core.Mocky
{
    public class MockyResultWrapper<T>
    {
        public MockyResultWrapper()
        {
            Products = new List<T>();
        }

        // TODO Refactor sending service to use "Data" in json or something instead of "Products", as list of Products is not generic
        // With generic class we could reuse this class for all Mocky responses
        public IEnumerable<T> Products { get; set; }

        public ApiKeysWrapper ApiKeys { get; set; }
    }
}
