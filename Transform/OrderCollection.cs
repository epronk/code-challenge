using System.Collections.Generic;
using Newtonsoft.Json;

namespace Retail
{
    public class OrderCollection
    {
        public long Id { get; set; }
        public string Vendor { get; set; }
        public string Date { get; set; }
        public string Customer { get; set; }
        [JsonProperty(Order = 1)]
        public List<Order> orders = new();
    }
}
