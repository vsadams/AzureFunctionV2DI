using AzureFunctionDI.Core.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Services.WebApi;
using System;

namespace AzureFunctionDI
{
	public static class FunctionTestAssemblyLoading
	{
		[FunctionName("Test")]
		public static void Test([TimerTrigger("0 */1 * * * *")]TimerInfo timer, ILogger logger, [Inject] VssConnection vssConnection)
		{
			try
			{


				logger.LogInformation($"Created vssConnection to {vssConnection.Uri}");
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Failed to create a vssconnection object");
				throw;
			}

		}
	}
}
