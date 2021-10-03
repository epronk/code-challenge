using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace Retail
{

    public class Transformer
    {
        private ReaderFactory _readerFactory;
        private JsonTextWriter _writer;
        JsonSerializer serializer = CreateSerializer();
        public Transformer(ReaderFactory readerFactory, JsonTextWriter writer)
        {
            _readerFactory = readerFactory;
            _writer = writer;
        }

        static JsonSerializer CreateSerializer()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };
             
            return serializer;
        }

        public void Process()
        {
            _writer.Formatting = Formatting.Indented;

            // { "customers": [
            _writer.WriteStartObject();
            _writer.WritePropertyName("customers");
            _writer.WriteStartArray();
            
            WriteCustomers();

            // ], "orders": [
            _writer.WriteEndArray();
            _writer.WritePropertyName("orders");
            _writer.WriteStartArray();

            WriteOrderCollection();

            // ] } }
            _writer.WriteEndArray();
            _writer.WriteEndObject();
            _writer.Flush();
        }
        public void WriteCustomers()
        {
            TextReader sr = _readerFactory.Create();
            var reader = new JsonTextReader(sr);
            var parser = new Retail.Parser(reader);
            parser.CustomerLoaded += this.OnCustomer;
            parser.ParseDoc();
        }
        public void WriteOrderCollection()
        {
            TextReader sr = _readerFactory.Create();
            var reader = new JsonTextReader(sr);
            var parser = new Retail.Parser(reader);
            parser.OrderCollectionLoaded += this.OnOrderCollection;
            parser.ParseDoc();
        }
        public void EndDocument()
        {
            _writer.WriteEndArray();
            _writer.WriteEndObject();
            _writer.Flush();
        }
        public void OnCustomer(object sender, Retail.Customer customer)
        {
            serializer.Serialize(_writer, customer);
        }
        public void OnOrderCollection(object sender, Retail.OrderCollection orderCollection)
        {
            serializer.Serialize(_writer, orderCollection);
        }
    }
}
