using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageQueue
{
    class Program
    {
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=az204akstorageaccount;AccountKey=68CKFqwGBPWts2oj1roo1Ibvs5jxmB2R96B/cvxMTTJabSjYIzYxLKxGgqBw+cdOmK4UaEknFi2YkA3MvpX5fg==;EndpointSuffix=core.windows.net";
        private static string queueName = "az204akqueue1";
        static void Main(string[] args)
        {
            QueueClient client = new QueueClient(connectionString,queueName);
            string message, tmpMessage;
            Byte[] txtByteMessage;
            //client.CreateIfNotExists();
            if (client.Exists())
            {
                //add message
                for (int i = 0; i < 5; i++)
                {
                    tmpMessage = $"Hello World {i}";
                    txtByteMessage = System.Text.Encoding.UTF8.GetBytes(tmpMessage);
                    message = Convert.ToBase64String(txtByteMessage);
                    client.SendMessage(message);
                    
                }

                //PeekMessage
                //Console.WriteLine(client.MaxPeekableMessages);
                //PeekedMessage[] messages = client.PeekMessages(2);
                //foreach (PeekedMessage message in messages)
                //{
                //    Console.WriteLine($"Message ID:{message.MessageId} ; Message Body :{message.Body}");
                //}

                //Receive message
                //QueueMessage queueMessage = client.ReceiveMessage();
                //Console.WriteLine(queueMessage.Body.ToString());
                //client.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);

            }
            
            Console.WriteLine($"All the messages are sent");
            Console.ReadLine();
        }
    }
}
