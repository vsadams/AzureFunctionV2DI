using AzureFunctionDI.Core.DependencyInjection.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace AzureFunctionDI
{
	public class StartupConfigProvider : IExtensionConfigProvider
	{
		/// <summary>
		/// This is invoked by the function framework at startup
		/// </summary>
		/// <param name="context"></param>
		public void Initialize(ExtensionConfigContext context)
		{
			var services = new ServiceCollection();
			RegisterVsts(services);

			var serviceProvider = services.BuildServiceProvider(true);
			context.AddInject(serviceProvider);

			AssemblyLoadContext.Default.Resolving += LoadAssembliesHandler;
		}

		private static void RegisterVsts(ServiceCollection services)
		{
			services.AddTransient(sp =>
			{
				var creds = new VssBasicCredential("REPLACE_WITH_USERNAME", "REPLACE_WITH_PASSWORD");
				var vssConnection = new VssConnection(new Uri("https://REPLACE_WITH_YOUR_URI"), creds);
				return vssConnection;
			});
		}

		private static Assembly LoadAssembliesHandler(AssemblyLoadContext context, AssemblyName assemblyName)
		{
			var libsThatFailToLoad = new List<string>
			{
				"System.Net.Http.Primitives",
				"Microsoft.IdentityModel.Clients.ActiveDirectory",
				"Microsoft.IdentityModel.Clients.ActiveDirectory.Platform"
			};

			if (libsThatFailToLoad.Contains(assemblyName.Name))
			{
				var executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
				var directory = Path.GetDirectoryName(executingAssemblyLocation);

				var assembly = Assembly.LoadFile($"{directory}/{assemblyName.Name}.dll");
				return assembly;
			}

			return null;
		}
	}
}
