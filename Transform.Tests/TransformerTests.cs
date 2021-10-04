using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Retail.Tests
{
    [TestClass]
    public class TransformerTests
    {
        [TestMethod]
        public void TestMinimalDocument()
        {
            var readerFactory = new Retail.StringReaderFactory("[]");
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            var writer = new JsonTextWriter(sw);

            var transformer = new Retail.Transformer(readerFactory, writer);
            transformer.Process();
            Assert.AreEqual(TestUtils.Normalize(@"{
  ""customers"": [],
  ""orders"": []
}"), sb.ToString());
        }
    }
}
