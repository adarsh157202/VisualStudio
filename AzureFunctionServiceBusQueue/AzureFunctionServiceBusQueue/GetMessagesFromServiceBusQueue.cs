using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionServiceBusQueue
{
    public static class GetMessagesFromServiceBusQueue
    {
        [FunctionName("GetMessagesFromServiceBusQueue")]
        public static void Run([ServiceBusTrigger("az204akservicebusqueue1", Connection = "con")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
