using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableConsoleApp1.models
{
    class Customer :TableEntity
    {
        public string customerName { get; set; }
        public Customer()
        {

        }
        public Customer(string id, string name, string city)
        {
            PartitionKey = city;
            RowKey = id;
            customerName = name;
        }
    }
}
