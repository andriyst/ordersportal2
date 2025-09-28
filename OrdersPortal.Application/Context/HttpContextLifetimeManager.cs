using System;
using System.Web;
using Unity.Lifetime;

namespace OrdersPortal.Application.Context
{
	public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
	{
		private readonly string _key;

		public HttpContextLifetimeManager()
		{
			_key = GetKey();
		}

		public override object GetValue(ILifetimeContainer container = null)
		{
			return HttpContext.Current.Items[_key];
		}

		public override void RemoveValue(ILifetimeContainer container = null)
		{
			HttpContext.Current.Items.Remove(_key);
		}
		public override void SetValue(object newValue, ILifetimeContainer container = null)
		{
			HttpContext.Current.Items[_key]
				= newValue;
		}
		public void Dispose()
		{
			RemoveValue();
		}

		public static string GetKey()
		{
			return typeof(T).AssemblyQualifiedName;
		}

		protected override LifetimeManager OnCreateLifetimeManager()
		{
			return new HttpContextLifetimeManager<T>();
		}
	}
}
