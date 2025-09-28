using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Unity;

namespace OrdersPortal.Infrastructure.DependencyInjection
{
	public class UnityDependencyResolver : IDependencyResolver
	{
		public IUnityContainer Container { get; private set; }

		public UnityDependencyResolver(IUnityContainer container)
		{
			Container = container;
		}

		public object GetService(Type serviceType)
		{
			try
			{
				return Container.Resolve(serviceType);
			}
			catch (Exception) { return null; }
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				return Container.ResolveAll(serviceType);
			}
			catch (Exception)
			{
				return new List<object>();
			}
		}
	}
}
