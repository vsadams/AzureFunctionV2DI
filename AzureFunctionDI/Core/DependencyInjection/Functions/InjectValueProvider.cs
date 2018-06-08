using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace AzureFunctionDI.Core.DependencyInjection.WebJobs
{
	class InjectValueProvider : IValueProvider
	{
		private readonly Type _type;
		private readonly object _explicitValue;
		private readonly IServiceProvider _serviceProvider;
		private readonly FunctionBindingContext _context;

		public InjectValueProvider(FunctionBindingContext context, IServiceProvider serviceProvider, Type type, object explicitValue = null)
		{
			_context = context;
			_serviceProvider = serviceProvider;
			_type = type;
			_explicitValue = explicitValue;
		}

		public Type Type => _explicitValue.GetType();

		public Task<object> GetValueAsync()
		{
			if (_explicitValue != null)
				return Task.FromResult(_explicitValue);

			var value = GetRequiredService(_type);
			return Task.FromResult(value);

		}

		public object GetRequiredService(Type type)
		{
			try
			{
				object value;
				if (type.IsAssignableFrom(typeof(IEnumerable)))
					value = _serviceProvider.GetServices(type);
				else
					value = _serviceProvider.GetRequiredService(type);

				if (value is IFunctionContextHandler contextHandler)
					contextHandler.FunctionBindingContext = _context;

				return value;
			}
			catch (Exception ex)
			{
				try
				{
					var logger = _serviceProvider.GetService<ILogger>();
					if (logger != null)
						logger.LogCritical(ex, "An error occured trying to retrive the type for {Type}", type);
				}
				catch { }
				throw;

			}
		}

		public string ToInvokeString() => _explicitValue?.ToString();
	}
}
