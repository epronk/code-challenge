using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Retail.Tests
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void TestSerialize()
        {
            var customer = new Retail.Customer();
            customer.Id = "8baa6dea-cc70-4748-9b27-b174e70e4b66";
	        customer.Name = "Lezlie Stuther";
            customer.Address = "19045 Lawn Court";
            //Assert.AreEqual("hat", order.Item);
            //Assert.AreEqual(112, order.Revenue);

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };

            Assert.AreEqual(@"{
  ""id"": ""8baa6dea-cc70-4748-9b27-b174e70e4b66"",
  ""name"": ""Lezlie Stuther"",
  ""address"": ""19045 Lawn Court""
}", JsonConvert.SerializeObject(customer, serializerSettings));
        }
    }
}
