using CosmosDbConsoleApp.models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;

namespace CosmosDbConsoleApp
{
    class Program
    {
        private static readonly string _connectionString = "AccountEndpoint=https://az204akcosmosdbaccount1.documents.azure.com:443/;AccountKey=7CyXRWdQdxhj9NcUB9pG7wGwdouSuJosTnAzeyEeYXClgfoMT1b46x6OjrYc0efyzgJQcNRyXZEEgfSsaPGK4g==;";
        private static readonly string _databaseName = "az204akcosmosdbdatabase1";
        private static readonly string _containerName = "course";
        private static readonly string _partitionKey = "/courseid";
        static void Main(string[] args)
        {
            try
            {
                CosmosClient client = new CosmosClient(_connectionString,new CosmosClientOptions() { AllowBulkExecution=true});
                //client.CreateDatabaseAsync(_databaseName).GetAwaiter().GetResult();
                Database database = client.GetDatabase(_databaseName);
                //database.CreateContainerAsync(_containerName, _partitionKey).GetAwaiter().GetResult();
                Container container = database.GetContainer(_containerName);
                //Course course = new Course()
                //{
                //    id="1",
                //    courseid="COURSE1",
                //    courseName="AZ900",
                //    rating=4.9m
                //};
                List<Course> courses = new List<Course>() {
                    new Course(){ id="2",courseid="Course2",courseName="AZ203",rating=4.7m},
                    new Course(){ id="3",courseid="Course3",courseName="AZ204",rating=4.5m},
                    new Course(){ id="4",courseid="Course4",courseName="AZ302",rating=4.6m},
                    new Course(){ id="5",courseid="Course5",courseName="AZ304",rating=4.8m}
                };
                List<T>
                //container.CreateItemAsync<Course>(course, new PartitionKey(course.courseid)).GetAwaiter().GetResult() ;
                Console.WriteLine("Item is created");
                Console.ReadLine();
                //Console.WriteLine("Hello World!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);            
            }
        }
    }
}
