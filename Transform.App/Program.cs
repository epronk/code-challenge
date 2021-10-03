using System.IO;
using Newtonsoft.Json;

namespace Retail
{
    class Program
    {
        static void Main(string[] args)
        {
            var readerFactory = new Retail.TextReaderFactory("data.json");
            StreamWriter file = File.CreateText("data-transformed.json");
            var writer = new JsonTextWriter(file);
            writer.Indentation = 4;
            var transformer = new Retail.Transformer(readerFactory, writer);

            transformer.Process();
        }
    }
}
