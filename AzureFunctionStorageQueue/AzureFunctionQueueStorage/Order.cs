using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionQueueStorage
{
    public class Order 
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
