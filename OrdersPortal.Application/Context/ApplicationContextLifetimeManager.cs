using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

using Unity.Lifetime;

namespace OrdersPortal.Application.Context
{
	public class ApplicationContextLifetimeManager : HttpContextLifetimeManager<ApplicationContext>, ITypeLifetimeManager
	{
		public override object GetValue(ILifetimeContainer container = null)
		{
			var applicationContext = base.GetValue() as ApplicationContext;

			if (applicationContext == null)
			{
				applicationContext = new ApplicationContext();

				if (HttpContext.Current.Request.IsAuthenticated)
				{
					string accountId = HttpContext.Current.User.Identity.GetUserId();
					applicationContext.AccountId = accountId;

					var accountRepository = DependencyResolver.Current.GetService<IAccountRepository>();
					OrderPortalUser orderPortalUser = accountRepository.GetByIdIncludes(accountId);
					if (orderPortalUser.Customer != null)
					{
						applicationContext.PermitFinanceInfo = orderPortalUser.Customer.PermitFinanceInfo ?? false;
						applicationContext.isCustomer = true;
					}
					else
					{
						applicationContext.PermitFinanceInfo = true;
						applicationContext.isCustomer = false;
					}

				}


				applicationContext.ReferrerUri = HttpContext.Current.Request.UrlReferrer;




				if (ApplicationContext.ApplicationPath == null)
				{
					if (HttpContext.Current.Request.ApplicationPath != null)
					{
						if (!HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
						{
							ApplicationContext.ApplicationPath = HttpContext.Current.Request.ApplicationPath + "/";
						}
						else
						{
							ApplicationContext.ApplicationPath = HttpContext.Current.Request.ApplicationPath;
						}
					}
				}

				SetValue(applicationContext);
			}

			return applicationContext;
		}


	}
}
