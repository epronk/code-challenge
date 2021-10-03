using System.IO;

namespace Retail
{
    // These factories exist because Transformer uses a two-pass parsing strategy.
    // The unit-tests can read input from a string.
    // The factories allow the transformer to ingest the input a second time.

    public abstract class ReaderFactory
    {
        public abstract TextReader Create();
    }
    public class TextReaderFactory : ReaderFactory
    {
        private string _filename;
        FileStream _stream;
        public TextReaderFactory(string filename)
        {
            _filename = filename;
        }
        public override TextReader Create()
        {
            if (_stream != null)
                _stream.Close();

            _stream = File.Open(_filename, FileMode.Open);
            StreamReader sr = new StreamReader(_stream);
            return sr;
        }
    }

    public class StringReaderFactory : ReaderFactory
    {
        private string _buffer;
        public StringReaderFactory(string s)
        {
            _buffer = s;
        }
        public override TextReader Create()
        {
            return new StringReader(_buffer);
        }
    }
}
