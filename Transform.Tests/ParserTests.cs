using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;


namespace Retail.Tests
{
    [TestClass]
    public class ParserTests
    {
        private Retail.Parser _parser;

        private void GivenParserInput(string s)
        {
            var readerFactory = new Retail.StringReaderFactory(s);
            TextReader sr = readerFactory.Create();
            var reader = new JsonTextReader(sr);
            _parser = new Retail.Parser(reader);
        }

        [TestMethod]
        public void TestEmptyDocument()
        {
            GivenParserInput("");

            // When / Then
            var ex = Assert.ThrowsException<JsonReaderException>(() => _parser.ParseDoc(), "");
            Assert.AreEqual("Unexpected end when reading JSON.", ex.Message);
        }

        [TestMethod]
        public void TestParseCustomer()
        {
            GivenParserInput(@"{
  ""id"" : ""8baa6dea-cc70-4748-9b27-b174e70e4b66"",
  ""name"": ""Lezlie Stuther"",
  ""address"": ""19045 Lawn Court""
}");

            // And
            var customer = new Retail.Customer();
            _parser.ParseCustomer(customer);

            // Then
            Assert.AreEqual("8baa6dea-cc70-4748-9b27-b174e70e4b66", customer.Id);
            Assert.AreEqual("Lezlie Stuther", customer.Name);
            Assert.AreEqual("19045 Lawn Court", customer.Address);
        }

        [TestMethod]
        public void TestParseOrders()
        {
            GivenParserInput(@"{
   ""hat"": {
    ""quantity"": 14,
    ""price"": 8
   }
}");

            // And
            var orders = new Retail.OrderCollection();

            // When
            _parser.ParseOrders(orders);

            // Then
            Assert.AreEqual("hat", orders.orders[0].Item);
            Assert.AreEqual(14, orders.orders[0].Quantity);
            Assert.AreEqual(8, orders.orders[0].Price);
        }

        [TestMethod]
        public void TestUnexpectedAttribute()
        {
            GivenParserInput(@"{ ""phone"" : ""1234567890"" } ");

            // And
            var customer = new Retail.Customer();

            // When / Then
            var ex = Assert.ThrowsException<JsonReaderException>(() => _parser.ParseCustomer(customer), "");
            Assert.AreEqual("Unexpected property 'phone' found.", ex.Message);
        }

        [TestMethod]
        public void TestParseCustomerAndOrderItem()
        {
            GivenParserInput(@"{ ""customer"": { ""id"": ""8baa6dea-cc70-4748-9b27-b174e70e4b66"" } }");
            bool customerLoaded = false;
            _parser.CustomerLoaded += (sender, customer) =>
            {
                // Then (there is some friction here. fixme: capture the object and assert this as part of the 'Then' clause
                Assert.AreEqual("8baa6dea-cc70-4748-9b27-b174e70e4b66", customer.Id);
                customerLoaded = true;
            };

            bool collectionLoaded = false;
            _parser.OrderCollectionLoaded += (sender, collection) =>
            {
                // Then (fixme: same as above)
                Assert.AreEqual("8baa6dea-cc70-4748-9b27-b174e70e4b66", collection.Customer);
                collectionLoaded = true;
            };

            // When 
            _parser.ParseItem();

            // Then
            Assert.IsTrue(customerLoaded);
            Assert.IsTrue(collectionLoaded);
        }
    }
}