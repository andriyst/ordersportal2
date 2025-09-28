using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OrdersPortal.Application.Services
{
	public class AnalyticsService : IAnalyticsService
	{

		private readonly IOrderRepository _orderRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IAccountService _accountService;
		private readonly ICustomersServices _customersServices;
		private readonly ApplicationContext _applicationContext;

		private const int InitComplaintStatusId = 1;

		public AnalyticsService(IOrderRepository orderRepository, IAccountRepository accountRepository, IAccountService accountService, ICustomersServices customersServices, ApplicationContext applicationContext)
		{
			_orderRepository = orderRepository;
			_accountRepository = accountRepository;
			_accountService = accountService;
			_customersServices = customersServices;
			_applicationContext = applicationContext;
		}

		public OrderAnalyticsListViewModel PrepareAnalyticsOrdersListViewModel(OrderAnalyticsListViewModel viewModel)
		{
			if (viewModel == null)
			{
				viewModel = new OrderAnalyticsListViewModel();
			}
			try
			{
				viewModel = (OrderAnalyticsListViewModel)InitDropDown(viewModel);
				if (_applicationContext.isCustomer)
				{
					viewModel.ContrAgentCode = _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return viewModel;
		}

		public List<AnalyticsCategoryOrder> GetOrderAnalyticsList(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			List<AnalyticsCategoryOrder> result = new List<AnalyticsCategoryOrder>();

			result = _customersServices.GetOrderAnalyticsList(contrAgentCode, startDate, endDate);


			return result;
		}

		public List<AnalyticChartModel> GetOrderAnalyticsChartsData(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			List<AnalyticChartModel> result = new List<AnalyticChartModel>();



			List<AnalyticsCategoryOrder> dataFrom1c = _customersServices.GetOrderAnalyticsList(contrAgentCode, startDate, endDate);

			foreach (var category in dataFrom1c) {
				AnalyticChartModel chartModel = new AnalyticChartModel();
				chartModel.Name = category.Category;
				chartModel.Title = category.Category;
				List<ChartData> chartDataList = new List<ChartData>();

				int checkDays=0;

				switch (category.Category)
				{
					case "Категорія А (3-7 роб. днів)":
						checkDays = 7;
						break;
					case "Категорія В (10-14 роб. днів)":
						checkDays = 14;
						break;
					case "Категорія С (24 роб. дні)":
						checkDays = 24;
						break;					
				}			

				var inTime = category.AnalyticsOrderList.Where(x => x.ActualyProductionDays <= checkDays).Count();
				var outTime = category.AnalyticsOrderList.Where(x => x.ActualyProductionDays > checkDays).Count();

				chartDataList.Add(new ChartData
				{
					Name = "Вчасно",
					Value = inTime
				});
				chartDataList.Add(new ChartData
				{
					Name = "Протерміновано",
					Value = outTime
				});

				chartModel.ChartData = chartDataList;
				result.Add(chartModel);
			}


			return result;
		}

		public List<AnalyticsProfileSystem> GetProfileSystemAnalytics(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			List<AnalyticsProfileSystem> result = new List<AnalyticsProfileSystem>();

			result = _customersServices.GetAnalyticsProfileSystem(contrAgentCode, startDate, endDate);


			return result;
		}




		#region PrivateFunctions
		private AnalyticsBaseViewModel InitDropDown(AnalyticsBaseViewModel viewModel)
		{
			if (viewModel.StartDate < new DateTime(2000, 1, 1) || viewModel.EndDate < new DateTime(2000, 1, 1))
			{
				viewModel.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
				viewModel.EndDate = DateTime.Today;
			}

			viewModel.ContrAgentCodeList = new List<SelectListItem>();

			viewModel.ContrAgentCodeList.AddRange(_customersServices.GetCustomerCodesList()
																	.OrderBy(x => x.CustomerName)
																	.Select(v => new SelectListItem
																	{
																		Text = v.CustomerName.ToString(),
																		Value = v.Code.ToString()
																	}).ToList());

			viewModel.ContrAgentCodeList.Insert(0, new SelectListItem { Value = "", Text = "Оберіть контрагента" });

			return viewModel;
		}

		public IQueryable<T> GetSortAndSearchList<T>(IQueryable<T> list, TableDataModel tableDataModel) where T : AnalyticsOrder
		{
			IQueryable<T> searchQuery;
			if (string.IsNullOrEmpty(tableDataModel.Search))
			{
				searchQuery = list;
			}
			else
			{
				searchQuery = GetSearchQuery(list, tableDataModel.Search);
			}
			if (!string.IsNullOrEmpty(tableDataModel.Sort))
			{
				searchQuery = GetSortQuery(searchQuery, tableDataModel.Sort, tableDataModel.Order);
			}

			return searchQuery;

		}

		protected IQueryable<T> GetSearchQuery<T>(IQueryable<T> dbSet, string value) where T : class
		{
			Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();

			List<string> nestedFileds = GetTypeProperties<T>(out propertyInfos);

			IQueryable<T> query = dbSet;

			List<Expression> expressions = new List<Expression>();

			ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

			foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string)))
			{
				var containsExpression = CreateExpression(typeof(T), prop.Name, value, parameter);
				expressions.Add(containsExpression);
			}

			foreach (var field in nestedFileds)
			{
				var exp = CreateExpression(typeof(T), field, value, parameter);
				expressions.Add(exp);
			}


			IQueryable<T> result;

			if (expressions.Count == 0)
				result = query;
			else
			{

				Expression orExpression = expressions[0];

				Expression<Func<T, bool>> expressionNew = Expression.Lambda<Func<T, bool>>(
					orExpression, parameter);

				for (int i = 1; i < expressions.Count; i++)
				{

					expressionNew = OrElse<T>(expressionNew, Expression.Lambda<Func<T, bool>>(
								 expressions[i], parameter));
				}

				result = query.Where(expressionNew);
			}

			return result;
		}

		public static Expression<Func<T, bool>> OrElse<T>(
		Expression<Func<T, bool>> expr1,
	   Expression<Func<T, bool>> expr2)
		{
			var parameter = Expression.Parameter(typeof(T));

			var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
			var left = leftVisitor.Visit(expr1.Body);

			var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
			var right = rightVisitor.Visit(expr2.Body);

			return Expression.Lambda<Func<T, bool>>(
				Expression.OrElse(left, right), parameter);
		}

		private class ReplaceExpressionVisitor
	   : ExpressionVisitor
		{
			private readonly Expression _oldValue;
			private readonly Expression _newValue;

			public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
			{
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override Expression Visit(Expression node)
			{
				if (node == _oldValue)
					return _newValue;
				return base.Visit(node);
			}
		}
		private static MethodCallExpression CreateExpression(Type type, string propertyName, string propertyValue, ParameterExpression param)
		{
			MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
			var someValue = Expression.Constant(propertyValue, typeof(string));
			//var param = Expression.Parameter(type, "x");

			Expression body = param;

			foreach (var member in propertyName.Split('.'))
			{
				body = Expression.PropertyOrField(body, member);

			}

			var containsMethodExp = Expression.Call(body, containsMethod, someValue);

			return containsMethodExp;
			//  return Expression.Lambda(containsMethodExp, param);
		}

		protected IQueryable<T> GetSortQuery<T>(IQueryable<T> dbSet, string orderValue, string orderDirection) where T : class
		{
			IQueryable<T> query = dbSet;
			if (!PropertyExists<T>(orderValue))
			{
				return dbSet;
			}

			if (typeof(T).GetProperty(orderValue, BindingFlags.IgnoreCase |
													BindingFlags.Public | BindingFlags.Instance) == null)
			{
				return null;
			}
			ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
			Expression orderByProperty = Expression.Property(paramterExpression, orderValue);
			LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
			MethodInfo genericMethod;
			if (orderDirection == "desc")
			{
				genericMethod =
					OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
			}
			else
			{
				genericMethod =
					OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
			}
			object ret = genericMethod.Invoke(null, new object[] { query, lambda });
			return (IQueryable<T>)ret;

		}

		private static readonly MethodInfo OrderByMethod =
			typeof(Queryable).GetMethods().Single(method =>
				method.Name == "OrderBy" && method.GetParameters().Length == 2);

		private static readonly MethodInfo OrderByDescendingMethod =
			typeof(Queryable).GetMethods().Single(method =>
				method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

		private static bool PropertyExists<T>(string propertyName)
		{
			return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
													   BindingFlags.Public | BindingFlags.Instance) != null;
		}

		public static Expression<Func<TEntity, bool>> CreateSearchQuery<TEntity>(List<PropertyInfo> properties, string text)
		{
			if (string.IsNullOrWhiteSpace(text) || properties == null || properties.Count == 0)
			{
				return null;
			}

			// For comparison
			//Expression<Func<ProductContainer, bool>> exp = q => searchText.Any(x => q.Product.ProductTitle.ToLower().Contains(x));

			var expressions = new List<Expression>();

			var entity = Expression.Parameter(typeof(TEntity), "q");

			//search type
			var searchMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

			//search terms
			var searchTerms = Expression.Constant(text.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

			//search param
			var str = Expression.Parameter(typeof(string), "s");

			foreach (var property in properties.Where(
				x => x.GetCustomAttribute<NotMappedAttribute>() == null))
			{

				var entityProperty = Expression.Property(entity, property);
				var toLower = Expression.Call(entityProperty, "ToLower", Type.EmptyTypes);
				var contains = Expression.Call(toLower, searchMethod, str);

				var anyExpression = Expression.Lambda<Func<string, bool>>(contains, str);

				var any = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, searchTerms, anyExpression);

				expressions.Add(any);
			}

			var ors = expressions.Aggregate((x, y) => Expression.Or(x, y));

			var exp = Expression.Lambda<Func<TEntity, bool>>(ors, entity);
			return exp;
		}
		private static List<string> GetTypeProperties<T>(out Dictionary<string, PropertyInfo> commonProperties) where T : class
		{
			var existedProperties = typeof(T)
				.GetProperties()
				.ToList()
				.ToDictionary(p => p.Name);

			commonProperties = existedProperties;

			List<string> result = new List<string>();
			var additionaProperties = new Dictionary<string, PropertyInfo>();

			foreach (var property in existedProperties)
			{

				if (!property.Value.PropertyType.IsValueType
						 && !property.Value.PropertyType.IsPrimitive
						 && (property.Value.PropertyType.Namespace != null && property.Value.PropertyType.Namespace.Contains("Vikna"))
						 )
				{
					var myType1 = Type.GetType(String.Concat((property.Value.PropertyType.Namespace + "." + property.Key), ", ", "ViknaStyle.Portal.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
					if (myType1 != null)
					{
						additionaProperties = myType1.GetProperties().ToList().ToDictionary(p => p.Name);

						foreach (var addProp in additionaProperties.Where(x => x.Value.PropertyType.Name.Contains("String")))
						{
							result.Add(myType1.Name + '.' + addProp.Key);
						}
					}
				}

			}

			return result;


		}
		#endregion
	}
}
