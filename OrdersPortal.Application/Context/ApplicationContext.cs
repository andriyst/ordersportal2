using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Context
{
	public class ApplicationContext
	{
		public bool IsValid { get; set; }
		public string AccountId { get; set; }

		public int? RoleId { get; set; }
		public Uri CurrentUri { get; set; }
		public Uri ReferrerUri { get; set; }
		public string ClientIpAddress { get; set; }
		public string DefaultEmail { get; set; }
		public static Uri RootUri { get; set; }
		public static string ApplicationPath { get; set; }
		public bool PermitFinanceInfo { get; set; }
		public bool isCustomer { get; set; }

		public static Func<ApplicationContext, string> GetTitle { get; set; }

		private readonly IDictionary<string, object> _contextData;

		static ApplicationContext()
		{
			GetTitle = (c) =>
			{
				return String.Empty;
			};
		}

		public ApplicationContext()
		{

			_contextData = new Dictionary<string, object>();
		}

		// public Language CurrentLanguage { get; set; }

		public IDictionary<string, object> ContextData
		{
			get { return _contextData; }
		}

		public bool IsAuthenticated
		{
			get { return !String.IsNullOrEmpty(AccountId); }
		}

		public string Title
		{
			get { return GetTitle(this); }
		}
	}
}
