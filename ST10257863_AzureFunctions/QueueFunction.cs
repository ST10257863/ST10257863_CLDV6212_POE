using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ST10257863.Functions
{
	public static class QueueFunction
	{
		[Function("QueueFunction")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
			ILogger log)
		{
			string queueName = "st10257863queueservice";
			string message = req.Query["message"];

			if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
			{
				return new BadRequestObjectResult("Queue name and message must be provided.");
			}

			var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
			var queueServiceClient = new QueueServiceClient(connectionString);
			var queueClient = queueServiceClient.GetQueueClient(queueName);
			await queueClient.CreateIfNotExistsAsync();
			await queueClient.SendMessageAsync(message);

			return new OkObjectResult("Message added to queue");
		}
	}
}
