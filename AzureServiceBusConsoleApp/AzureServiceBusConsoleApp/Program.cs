using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;

namespace AzureServiceBusConsoleApp
{
    class Program
    {
        public static string connection_string = "Endpoint=sb://az204akservicebus1.servicebus.windows.net/;SharedAccessKeyName=az204akservicebusqueuesend;SharedAccessKey=nbtVIve/bfUEMMScxvk1pb2/NAHD3XwRlwI54iHh0Og=;EntityPath=az204akservicebusqueue1";
        public static string queue_name = "az204akservicebusqueue1";
        public static string connection_string_2 = "Endpoint=sb://az204akservicebus1.servicebus.windows.net/;SharedAccessKeyName=az204aksendandreceivepolicy;SharedAccessKey=yx+37tSWx/zQyCytq9I4zib8Sr+lVuK36ql4dprjpvs=;EntityPath=az204akservicebusqueue2";
        public static string queue_name2 = "az204akservicebusqueue2";
        public static string topic_connection_string = "Endpoint=sb://az204akservicebus1.servicebus.windows.net/;SharedAccessKeyName=az204aksaspolicy1;SharedAccessKey=KchNKZsECRzogqU1NmUT4dDERkuQlfIYHLDdYLBNbIU=;EntityPath=az204aktopic1";
        public static string topic_name = "az204aktopic1";
        public static string subscription_name1 = "az204aksubscription1";
        public static string subscription_name2 = "az204aksubscription2";
        static void Main(string[] args)
        {
            //sending message to service bus and same code is used for both topic and queues
            List<Order> orders = new List<Order>()
            {
                new Order(){OrderID="01",Quantity=10,UnitPrice=9.99m},
                new Order(){OrderID="02",Quantity=15,UnitPrice=8.99m},
                new Order(){OrderID="03",Quantity=20,UnitPrice=7.99m},
                new Order(){OrderID="04",Quantity=25,UnitPrice=6.99m},
                new Order(){OrderID="05",Quantity=30,UnitPrice=5.99m}
            };
            //ServiceBusClient client = new ServiceBusClient(connection_string);
            ServiceBusClient client = new ServiceBusClient(topic_connection_string);
            //ServiceBusSender sender = client.CreateSender(queue_name);
            ServiceBusSender sender = client.CreateSender(topic_name);
            foreach (Order order in orders)
            {
                ServiceBusMessage serviceBusMessage = new ServiceBusMessage(order.ToString());
                serviceBusMessage.TimeToLive = TimeSpan.FromSeconds(30);
                sender.SendMessageAsync(serviceBusMessage).GetAwaiter().GetResult();
            }

            //sending duplicate messages
            //ServiceBusClient serviceBusClient = new ServiceBusClient(connection_string_2);
            //ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queue_name2);
            //ServiceBusMessage msg1 = new ServiceBusMessage("Hello World");
            //msg1.MessageId = "1";
            //serviceBusSender.SendMessageAsync(msg1).GetAwaiter().GetResult();



            ////peeking messages
            //ServiceBusClient client = new ServiceBusClient(connection_string);
            //ServiceBusReceiver serviceBusReceiver = client.CreateReceiver(queue_name, new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.PeekLock });
            //ServiceBusReceivedMessage serviceBusReceivedMessage = serviceBusReceiver.ReceiveMessageAsync().GetAwaiter().GetResult();

            ////for deleting after peek a message use
            //serviceBusReceiver.CompleteMessageAsync(serviceBusReceivedMessage);
            //Console.WriteLine(serviceBusReceivedMessage.Body.ToString());

            //Receive messages and the code is same for queuse and topics 
            //ServiceBusClient client = new ServiceBusClient(topic_connection_string);
            //ServiceBusReceiver serviceBusReceiver = client.CreateReceiver(topic_name,subscription_name1,new ServiceBusReceiverOptions() {ReceiveMode=ServiceBusReceiveMode.ReceiveAndDelete });
            //IReadOnlyList<ServiceBusReceivedMessage> serviceBusReceivedMessages = serviceBusReceiver.ReceiveMessagesAsync(20).GetAwaiter().GetResult();
            //foreach (ServiceBusReceivedMessage serviceBusReceivedMessage in serviceBusReceivedMessages)
            //{
            //    Console.WriteLine(serviceBusReceivedMessage.Body.ToString());
            //}


            Console.ReadLine();
            
        }
    }
}
