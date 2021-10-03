using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Retail.Tests
{
    [TestClass]
    public class ReaderFactoryTests
    {
        [TestMethod]
        public void TestSerialize()
        {
            var readerFactory = new Retail.TextReaderFactory("Transform.Tests.deps.json");
            var reader = readerFactory.Create();
            reader = readerFactory.Create();
        }
    }
}
