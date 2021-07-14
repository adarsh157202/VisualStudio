using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableConsoleApp1.models
{
    class Customer2:TableEntity
    {
        public string customerName { get; set; }
        public Customer2()
        {

        }
        public bool isRegularCustomer { get; set; }
        public Customer2(string id, string name, string city,bool isRegular)
        {
            PartitionKey = city;
            RowKey = id;
            customerName = name;
            isRegularCustomer = isRegular;
        }
    }
}
