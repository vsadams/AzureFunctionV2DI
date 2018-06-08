using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Protocols;
using System;
using System.Threading.Tasks;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	public class InjectBinding : IBinding
	{
		private readonly Type _type;
		private readonly IInjectBindingProvider _injectBindingProvider;

		public InjectBinding(IInjectBindingProvider injectBindingProvider, Type type)
		{
			_type = type;
			_injectBindingProvider = injectBindingProvider;
		}

		public bool FromAttribute => true;

		public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
		{
			var serviceProvider = _injectBindingProvider.GetServiceProvider(context.FunctionContext);
			var valueProvider = new InjectValueProvider(context.FunctionContext, serviceProvider, _type, value);
			return Task.FromResult<IValueProvider>(valueProvider);
		}

		public async Task<IValueProvider> BindAsync(BindingContext context)
		{
			return await BindAsync(null, context.ValueContext).ConfigureAwait(false);
		}

		public ParameterDescriptor ToParameterDescriptor() => new ParameterDescriptor();
	}
}
