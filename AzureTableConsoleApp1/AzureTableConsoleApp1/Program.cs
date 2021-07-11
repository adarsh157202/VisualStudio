using Microsoft.Azure.Cosmos.Table;
using System;

namespace AzureTableConsoleApp1
{
    class Program
    {
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=az204akstorageaccount1;AccountKey=u36QJ42GHAH49lgkd5ThvEsO+23hLnr4+fgIyZ5CyAb8gJUCtomv5Vq+BUhbE1B+iiRv7lbqMJhox8CGHCNASg==;EndpointSuffix=core.windows.net";
        private static string tableName = "Customers2";
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            CloudStorageAccount account =  CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = account.CreateCloudTableClient();
            CloudTable cloudTable =cloudTableClient.GetTableReference(tableName);
            cloudTable.CreateIfNotExists();
            Console.WriteLine("Table is created");
            Console.ReadLine();
        }
    }
}
