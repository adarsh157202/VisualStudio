using AzureTableConsoleApp1.models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace AzureTableConsoleApp1
{
    class Program
    {
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=az204akstorageaccount1;AccountKey=u36QJ42GHAH49lgkd5ThvEsO+23hLnr4+fgIyZ5CyAb8gJUCtomv5Vq+BUhbE1B+iiRv7lbqMJhox8CGHCNASg==;EndpointSuffix=core.windows.net";
        private static string tableName = "Customers2";
        private static string partitionKey="Ranchi";
        private static string rowKey = "cust1";
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            try
            {
                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                //Table creation
                CloudTableClient cloudTableClient = account.CreateCloudTableClient();
                CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);
                cloudTable.CreateIfNotExists();
                //ading one entity
                //Customer customer = new Customer("cust1", "Adarsh", "Ranchi");
                //TableOperation operation = TableOperation.Insert(customer);
                //TableResult result = cloudTable.Execute(operation);
                //Console.WriteLine($"The table is inserted {result.Result}");

                //performing batch operation
                //List<Customer> customes = new List<Customer>()
                //{
                //    new Customer("cust2", "Atul", "Ranchi"),
                //    new Customer("cust3", "Sanket", "Ranchi"),
                //    new Customer("cust4", "Anurag", "Ranchi")
                //};
                //TableBatchOperation tableOperations = new TableBatchOperation();
                //foreach (Customer customer in customes)
                //{
                //    tableOperations.Insert(customer);
                //}
                //TableBatchResult results=cloudTable.ExecuteBatch(tableOperations);

                //adding entity of different class                
                //Customer2 customer2 = new Customer2("cust5", "Atishay", "Ranchi",true);
                //TableOperation operation = TableOperation.Insert(customer2);
                //TableResult result = cloudTable.Execute(operation);
                //Console.WriteLine($"The table is inserted {result.Result}");

                //Retreiving an entity
                TableOperation tableOperation = TableOperation.Retrieve<Customer>(partitionKey, rowKey);
                TableResult result=cloudTable.Execute(tableOperation);
                Customer customer = result.Result as Customer;
                Console.WriteLine($"Customer id ={customer.RowKey} ; Customer Name ={customer.customerName} ; Customer City ={customer.PartitionKey }");
                Customer2 customer2 = result.Result as Customer2;
                Console.WriteLine($"Customer id ={customer2.RowKey} ; Customer Name ={customer2.customerName} ; Customer City ={customer2.PartitionKey } ;IsRegularCustomer ={customer2.isRegularCustomer}");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception occured {ex}");
            }
        }
    }
}
