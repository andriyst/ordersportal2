using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Configurations;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;
using OrdersPortal.Infrastructure.DependencyInjection;
using OrdersPortal.Infrastructure.EntityFramework;
using OrdersPortal.Infrastructure.Repositories;
using OrdersPortal.Services.Services;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace OrdersPortal.WebUI
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var unityContainer = InitUnityContainer();

			var unityDependencyResolver = new UnityDependencyResolver(unityContainer);
			DependencyResolver.SetResolver(unityDependencyResolver);

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			Database.SetInitializer<OrderPortalDbContext>(new OrdersPortalDBInitializer());

			string culture = "uk-UA";
			Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
			Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);

		}

		private static IUnityContainer InitUnityContainer()
		{
			var container = new UnityContainer();

			container.RegisterType<IUnitOfWork, UnitOfWork>();
			container.RegisterType<UnitOfWork>(new PerResolveLifetimeManager());

			container.RegisterType<IDbContextProvider, OrdersPortalDbContextProvider>(new InjectionConstructor("OrdersPortalConnection"));
			container.RegisterType<ApplicationContext>(new ApplicationContextLifetimeManager());

			#region Dependency Injection for Repositories

			container.RegisterType<IRegionRepository, RegionRepository>();
			container.RegisterType<IOrderRepository, OrderRepository>();
			container.RegisterType<IAccountRepository, AccountRepository>();
			container.RegisterType<IMessageRepository, MessageRepository>();
			container.RegisterType<IStatusRepository, StatusRepository>();
			container.RegisterType<IDb1SOrderNumbersRepository, Db1SOrderNumbersRepository>();
			container.RegisterType<IComplaintsRepository, ComplaintsRepository>();
			container.RegisterType<IComplaintSolutionRepository, ComplaintSolutionRepository>();
			container.RegisterType<IComplaintIssueRepository, ComplaintIssueRepository>();
			container.RegisterType<IComplaintOrderSeriesRepository, ComplaintOrderSeriesRepository>();
			container.RegisterType<IComplaintPhotoRepository, ComplaintPhotoRepository>();
			container.RegisterType<IHelpServiceContactRepository, HelpServiceContactRepository>();
			container.RegisterType<IHelpServiceLogRepository, HelpServiceLogRepository>();
			container.RegisterType<IVideoContentRepository, VideoContentRepository>();
			container.RegisterType<IFilesRepository, FilesRepository>();

			container.RegisterType<IOrderPartsRepository, OrderPartsRepository>();
			container.RegisterType<IOrderPartsItemRepository, OrderPartsItemRepository>();
			container.RegisterType<IOrderPartsReasonRepository, OrderPartsReasonRepository>();
			container.RegisterType<IDb1SOrderPartsNumbersRepository, Db1SOrderPartsNumbersRepository>();
			container.RegisterType< IOrganizationRepository, OrganizationRepository>();
			#endregion

			#region Dependency Injection for Services

			container.RegisterType<IRegionService, RegionService>();
			container.RegisterType<IOrderService, OrderService>();
			container.RegisterType<IAccountService, AccountService>();
			container.RegisterType<IMessageService, MessageService>();
			container.RegisterType<IComplaintsService, ComplaintsService>();
			container.RegisterType<ICustomersServices, CustomersServices>();
			container.RegisterType<IWebOrderServices, WebOrderServices>();
			container.RegisterType<IHelpServiceService, HelpServiceService>();
			container.RegisterType<IStatService, StatService>();
			container.RegisterType<IFinanceService, FinanceService>();
			container.RegisterType<IVideoContentService, VideoContentService>();
			container.RegisterType<IFilesService, FilesService>();
			container.RegisterType<IOrderPartsService, OrderPartsService>();
			container.RegisterType<IAnalyticsService, AnalyticsService>();

			container.RegisterType<IOrganizationService, OrganizationService>();

			#endregion


			var applicationConfiguration = new ApplicationConfiguration
			{
				// This part will remain in app.config, we can use interfaces or different configuration classes for this
				BaseUrl = ConfigurationManager.AppSettings["BaseUrl"],
				DefaultEmail = ConfigurationManager.AppSettings["SmtpStandartEmailAddress"],
				Language = "ua"
			};
			container.RegisterInstance(applicationConfiguration);
			return container;
		}
	}
}
