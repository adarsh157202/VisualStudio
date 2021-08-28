using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AzureFunctionQueueStorage
{
    public static class QueueMessage
    {
        [FunctionName("GetMessage")]
        [return:Table("Orders" ,Connection = "con")]
        public static Order Run([QueueTrigger("az204akqueue1", Connection = "con")]JObject myQueueItem,
            [Blob("")]
            ILogger log)
        {
            Order order=new Order();
            order.PartitionKey = myQueueItem["Category"].ToString();
            order.RowKey = myQueueItem["OrderID"].ToString();
            order.Quantity = Convert.ToInt32(myQueueItem["Quantity"]);
            order.UnitPrice = Convert.ToDecimal(myQueueItem["UnitPrice"]);
            //log.LogInformation ($"C# Queue trigger function processed: {myQueueItem}");
            return order;

        }
    }
}
