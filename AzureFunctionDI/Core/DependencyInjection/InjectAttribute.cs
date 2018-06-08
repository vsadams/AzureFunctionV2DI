using Microsoft.Azure.WebJobs.Description;
using System;

namespace AzureFunctionDI.Core.DependencyInjection
{
	[Binding]
	public class InjectAttribute : Attribute
	{
	}
}
