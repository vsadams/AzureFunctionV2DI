using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Config;
using System;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	public static class ExtensionConfigContextExtensions
	{

		public static void AddInject(this ExtensionConfigContext context, IServiceProvider serviceProvider)
		{
			context
				   .AddBindingRule<InjectAttribute>()
				   .Bind(new InjectBindingProvider(serviceProvider));

			var registry = context.Config.GetService<IExtensionRegistry>();

			var filter = new ScopeCleanupFilter();
			registry.RegisterExtension(typeof(IFunctionInvocationFilter), filter);
			registry.RegisterExtension(typeof(IFunctionExceptionFilter), filter);
		}
	}
}
