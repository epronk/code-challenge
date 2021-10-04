using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Retail.Tests
{
    [TestClass]
    public class OrderCollectionTests
    {
        [TestMethod]
        public void TestSerialize()
        {
            var order = new Retail.Order();
            order.Item = "hat";
            order.Quantity = 14;
            order.Price = 8;

            var orders = new Retail.OrderCollection();
            orders.Id = 1;
            orders.Vendor = "acme";
            orders.Date = "03/03/2017";
            orders.Customer = "8baa6dea-cc70-4748-9b27-b174e70e4b66";
            orders.orders.Add(order);

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };

            Assert.AreEqual(TestUtils.Normalize(@"{
  ""id"": 1,
  ""vendor"": ""acme"",
  ""date"": ""03/03/2017"",
  ""customer"": ""8baa6dea-cc70-4748-9b27-b174e70e4b66"",
  ""orders"": [
    {
      ""item"": ""hat"",
      ""quantity"": 14,
      ""price"": 8,
      ""revenue"": 112
    }
  ]
}"), JsonConvert.SerializeObject(orders, serializerSettings));

        }
    }
}
