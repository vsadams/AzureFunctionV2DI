using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	public class InjectBindingProvider : IBindingProvider, IInjectBindingProvider
	{
		private readonly IServiceProvider _serviceProvider;

		private static readonly ConcurrentDictionary<Guid, IServiceScope> Scope = new ConcurrentDictionary<Guid, IServiceScope>();

		public InjectBindingProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public Task<IBinding> TryCreateAsync(BindingProviderContext context)
		{
			var binding = new InjectBinding(this, context.Parameter.ParameterType);
			return Task.FromResult<IBinding>(binding);
		}

		public IServiceProvider GetServiceProvider(FunctionBindingContext context)
		{
			var scope = Scope.GetOrAdd(context.FunctionInstanceId, (id) =>
			{
				return _serviceProvider.CreateScope();
			});
			return scope.ServiceProvider;
		}


		public static void RemoveScope(Guid scopeId)
		{
			if (Scope.TryRemove(scopeId, out var scope))
			{
				scope.Dispose();
			}
		}
	}
}
