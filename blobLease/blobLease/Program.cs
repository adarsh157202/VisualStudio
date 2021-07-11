using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.IO;

namespace blobLease
{
    class Program
    {
        private static string _container_name = "az204akblobcontainer2";
        private static string _connection_string = "DefaultEndpointsProtocol=https;AccountName=az204akstorageaccount1;AccountKey=u36QJ42GHAH49lgkd5ThvEsO+23hLnr4+fgIyZ5CyAb8gJUCtomv5Vq+BUhbE1B+iiRv7lbqMJhox8CGHCNASg==;EndpointSuffix=core.windows.net";
        private static string _blob_name = "blob_file.txt";
        public static string _blob_location = "C:\\udemy\\blob_file.txt";
        static void Main(string[] args)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_connection_string);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(_container_name);
            BlobClient blobClient = blobContainerClient.GetBlobClient(_blob_name);

            MemoryStream memory = new MemoryStream();
            blobClient.DownloadTo(memory);
            memory.Position = 0;
            StreamReader reader = new StreamReader(memory);

            BlobLeaseClient blobLeaseClient = blobClient.GetBlobLeaseClient();
            BlobLease blobLease = blobLeaseClient.Acquire(TimeSpan.FromSeconds(60));
            Console.WriteLine($"{blobLease.LeaseId}");

            BlobUploadOptions blobUploadOptions = new BlobUploadOptions()
            {
                Conditions =new BlobRequestConditions()
                {
                    LeaseId = blobLease.LeaseId
                }
            };

            Console.WriteLine($"{reader.ReadToEnd()}");
            StreamWriter writer = new StreamWriter(memory);
            writer.WriteLine($"Now we are using memory stream #change2");
            writer.Flush();
            memory.Position = 0;
            blobClient.Upload(memory, blobUploadOptions);
            blobLeaseClient.Release();

            Console.WriteLine($"Change made");
            Console.ReadLine();
        }
    }
}
