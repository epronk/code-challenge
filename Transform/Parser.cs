using System;
using Newtonsoft.Json;

namespace Retail
{
    public class Parser
    {
        public event EventHandler<Customer> CustomerLoaded;
        public event EventHandler<OrderCollection> OrderCollectionLoaded;

        protected void OnCustomer(Customer customer)
        {
            CustomerLoaded?.Invoke(this, customer);
        }
        protected void OnOrderCollection(OrderCollection collection)
        {
            OrderCollectionLoaded?.Invoke(this, collection);
        }

        internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
        {
            return message;
        }

        internal static JsonReaderException CreateException(Parser parser, string message)
        {
            return Create(parser, message, null);
        }

        internal static JsonReaderException Create(Parser parser, string message, Exception ex)
        {
            return CreateException(parser._reader as IJsonLineInfo, parser._reader.Path, message, ex);
        }
        internal static JsonReaderException CreateException(IJsonLineInfo lineInfo, string path, string message, Exception ex)
        {
            message = FormatMessage(lineInfo, path, message);

            int lineNumber;
            int linePosition;
            if (lineInfo != null && lineInfo.HasLineInfo())
            {
                lineNumber = lineInfo.LineNumber;
                linePosition = lineInfo.LinePosition;
            }
            else
            {
                lineNumber = 0;
                linePosition = 0;
            }

            return new JsonReaderException(message, path, lineNumber, linePosition, ex);
        }

        public Parser(JsonReader reader)
        {
            _reader = reader;
        }

        internal JsonReaderException CreateUnexpectedEndException()
        {
            return CreateException(this, "Unexpected end when reading JSON.");
        }

        internal JsonReaderException CreateUnexpectedPropertyException(string propertyName)
        {
            throw CreateException(this, $"Unexpected property '{propertyName}' found.");
        }

        internal void ReaderReadAndAssert()
        {
            if (!_reader.Read())
            {
                throw CreateUnexpectedEndException();
            }
        }

        public void ParseCustomer(Customer customer)
        {
            ReaderReadAndAssert();

            if (_reader.TokenType != JsonToken.StartObject)
                throw CreateException(this, "expected start object.");
            ReaderReadAndAssert();

            while (_reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = _reader.Value.ToString().ToLower();
                ReaderReadAndAssert();
                if (propertyName.Equals("id"))
                    customer.Id = _reader.Value.ToString();
                else if (propertyName.Equals("name"))
                    customer.Name = _reader.Value.ToString();
                else if (propertyName.Equals("address"))
                    customer.Address = _reader.Value.ToString();
                else
                    throw CreateUnexpectedPropertyException(propertyName);
                ReaderReadAndAssert();
            }
            if (_reader.TokenType != JsonToken.EndObject)
                throw CreateException(this, "expected end object.");

            OnCustomer(customer);
        }
        public void ParseOrders(OrderCollection collection)
        {
            ReaderReadAndAssert();
            if (_reader.TokenType != JsonToken.StartObject)
                throw CreateException(this, "expected start object.");

            ReaderReadAndAssert();
            while (_reader.TokenType == JsonToken.PropertyName)
            {
                var order = new Retail.Order();
                order.Item = _reader.Value.ToString();
                ReaderReadAndAssert();
                if (_reader.TokenType != JsonToken.StartObject)
                    throw CreateException(this, "expected start object.");

                ReaderReadAndAssert();

                while (_reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = _reader.Value.ToString().ToLower();
                    ReaderReadAndAssert();
                    if (propertyName.Equals("quantity"))
                        order.Quantity = Convert.ToInt64(_reader.Value);
                    else if (propertyName.Equals("price"))
                        order.Price = Convert.ToInt64(_reader.Value);
                    else
                        throw CreateUnexpectedPropertyException(propertyName);
                    ReaderReadAndAssert();
                }
                collection.orders.Add(order);
                ReaderReadAndAssert();
            }
            if (_reader.TokenType != JsonToken.EndObject)
                throw CreateException(this, "expected end object.");
        }
        public void ParseItem()
        {
            ReaderReadAndAssert();
            if (_reader.TokenType == JsonToken.EndArray)
                return;

            if (_reader.TokenType != JsonToken.StartObject)
                throw CreateException(this, "expected start object.");
            ReaderReadAndAssert();
            var collection = new Retail.OrderCollection();
            var customer = new Retail.Customer();

            while (_reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = _reader.Value.ToString().ToLower();

                if (propertyName.Equals("customer"))
                    ParseCustomer(customer);
                else if (propertyName.Equals("order"))
                    ParseOrders(collection);
                else
                {
                    ReaderReadAndAssert();
                    if (propertyName.Equals("id"))
                        collection.Id = Convert.ToInt64(_reader.Value);
                    else if (propertyName.Equals("vendor"))
                        collection.Vendor = _reader.Value.ToString();
                    else if (propertyName.Equals("date"))
                        collection.Date = _reader.Value.ToString();
                    else
                        throw CreateUnexpectedPropertyException(propertyName);
                }
                ReaderReadAndAssert();
            }
            collection.Customer = customer.Id;
            OnOrderCollection(collection);

            if (_reader.TokenType != JsonToken.EndObject)
                throw CreateException(this, "expected end object.");
        }

        public void ParseDoc()
        {
            ReaderReadAndAssert();

            while (_reader.TokenType != JsonToken.EndArray)
            {
                ParseItem();
            }
        }

        private JsonReader _reader;
    }
}
