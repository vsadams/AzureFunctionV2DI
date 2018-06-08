using Microsoft.Azure.WebJobs.Host.Bindings;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	interface IFunctionContextHandler
	{
		FunctionBindingContext FunctionBindingContext { set; }
	}
}
