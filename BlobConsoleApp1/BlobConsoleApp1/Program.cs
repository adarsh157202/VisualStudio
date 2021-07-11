using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobConsoleApp1
{
    class Program
    {
        private static string _container_name = "az204akblobcontainer2";
        private static string _connection_string = "DefaultEndpointsProtocol=https;AccountName=az204akstorageaccount1;AccountKey=u36QJ42GHAH49lgkd5ThvEsO+23hLnr4+fgIyZ5CyAb8gJUCtomv5Vq+BUhbE1B+iiRv7lbqMJhox8CGHCNASg==;EndpointSuffix=core.windows.net";
        private static string _blob_name = "blob_file.txt";
        public static string _blob_location = "C:\\udemy\\blob_file.txt";
        static void Main(string[] args)
        {

            //Console.WriteLine("Hello World!");
            //BlobServiceClient _service_client = new BlobServiceClient(_connection_string);
            //BlobContainerClient _container_client = new BlobContainerClient(_connection_string,_container_name);

            //_service_client.CreateBlobContainer(_container_name);
            //Console.WriteLine("Blob container "+_container_name +" created");

            //BlobClient blobClient = new BlobClient(_connection_string, _container_name, _blob_name);
            //blobClient.Upload(_blob_location);
            //Console.WriteLine("Blob is uploaded");

            //BlobServiceClient serviceClient = new BlobServiceClient(_connection_string);
            ////Azure.Pageable<BlobContainerItem> _containers = serviceClient.GetBlobContainers();
            //foreach (BlobContainerItem container in serviceClient.GetBlobContainers())
            //{
            //    Console.WriteLine(container.Name);
            //    BlobContainerClient _container_client = serviceClient.GetBlobContainerClient(container.Name);
            //    foreach(BlobItem blob in _container_client.GetBlobs())
            //    {
            //        Console.WriteLine(blob.Name);
            //        if(blob.Name==_blob_name)
            //        {
            //            BlobClient blobClient = _container_client.GetBlobClient(blob.Name);
            //            //blobClient.DownloadToAsync(_blob_location);
            //            Azure.Response<BlobDownloadResult> response=blobClient.DownloadContent();
            //            Azure.Response<BlobDownloadStreamingResult> res=blobClient.DownloadStreaming();
            //        }
            //    }
            //}

            //Uri blobUri=GenerateSAS(_connection_string,_container_name,_blob_name);
            //BlobClient blobClient = new BlobClient(blobUri);
            //blobClient.DownloadTo(_blob_location);

            BlobServiceClient blobServiceClient = new BlobServiceClient(_connection_string);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(_container_name);
            BlobClient blobClient = blobContainerClient.GetBlobClient(_blob_name);
            BlobProperties properties = blobClient.GetProperties();
            
            Console.WriteLine($"created on ={properties.CreatedOn}");
            Console.WriteLine($"encryption key on ={properties.EncryptionKeySha256}");

            IDictionary<string ,string> metadatas=properties.Metadata;
            
            foreach(KeyValuePair<string,string> metadata in metadatas)
            {
                Console.WriteLine($"Key ={metadata.Key}");
                Console.WriteLine($"Value ={metadata.Value}");
            }
            GetBlobTagResult tags=blobClient.GetTags();
            Console.WriteLine($"{tags.Tags}");
            string key = "Company",value="Deloitte";

            bool response=SetMetadata(blobClient,metadatas,key,value);
            Console.ReadLine();
        }
        public static bool SetMetadata(BlobClient blobClient,IDictionary<string,string>metaData,string key,string value)
        {
            try
            {
                metaData.Add(key,value);
                blobClient.SetMetadata(metaData);
                return true;
            }
            catch (Exception ex) {
                return false;            
            }
        }
        public static Uri GenerateSAS(string connectionString, string containerName, string blobName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b"
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read| BlobSasPermissions.List);
            sasBuilder.StartsOn = DateTimeOffset.UtcNow;
            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(20);
            return blobClient.GenerateSasUri(sasBuilder);
        }

        
        
    }
}
