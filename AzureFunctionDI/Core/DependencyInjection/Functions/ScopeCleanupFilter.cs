using Microsoft.Azure.WebJobs.Host;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	public class ScopeCleanupFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
	{
		public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
		{
			InjectBindingProvider.RemoveScope(exceptionContext.FunctionInstanceId);
			return Task.CompletedTask;
		}

		public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
		{
			InjectBindingProvider.RemoveScope(executedContext.FunctionInstanceId);
			return Task.CompletedTask;
		}

		public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) =>
			Task.CompletedTask;
	}
}
