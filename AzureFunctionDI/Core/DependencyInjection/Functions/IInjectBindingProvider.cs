using Microsoft.Azure.WebJobs.Host.Bindings;
using System;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	public interface IInjectBindingProvider
	{
		IServiceProvider GetServiceProvider(FunctionBindingContext context);
	}
}