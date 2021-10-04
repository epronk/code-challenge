using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Retail.Tests
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void TestSerialize()
        {
            var order = new Retail.Order();
            order.Item = "hat";
            order.Quantity = 14;
            order.Price = 8;
            Assert.AreEqual("hat", order.Item);
            Assert.AreEqual(112, order.Revenue);

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
  ""item"": ""hat"",
  ""quantity"": 14,
  ""price"": 8,
  ""revenue"": 112
}"), JsonConvert.SerializeObject(order, serializerSettings));
        }
    }
}
