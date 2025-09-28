using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Dto.Customer1cOrder;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;

namespace OrdersPortal.Application.Services
{
	public class FinanceService : IFinanceService
	{
		private readonly IAccountRepository _accountRepository;
		private readonly ICustomersServices _customersServices;
		private readonly IWebOrderServices _webOrderServices;
		private readonly ApplicationContext _applicationContext;
		private readonly DateTime _initWdsDate;


		public FinanceService(IAccountRepository accountRepository, ICustomersServices customersServices, IWebOrderServices webOrderServices, ApplicationContext applicationContext)
		{
			_accountRepository = accountRepository;
			_customersServices = customersServices;
			_webOrderServices = webOrderServices;
			_applicationContext = applicationContext;
			_initWdsDate = new DateTime(2025, 03, 01);
		}


		public FinanceOrdersListViewModel PrepareFinanceOrdersListViewModel()
		{
			var viewModel = new FinanceOrdersListViewModel();
			try
			{
				viewModel = (FinanceOrdersListViewModel)InitDropDown(viewModel);
				if (_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo)
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

		public FinanceCashFlowViewModel PrepareFinanceCashFlowViewModel(FinanceCashFlowViewModel viewModel)
		{
			if (viewModel == null)
			{
				viewModel = new FinanceCashFlowViewModel();
			}
			try
			{
				viewModel = (FinanceCashFlowViewModel)InitDropDown(viewModel);
				if (_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo)
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

		public FinanceReconciliationViewModel PrepareFinanceReconciliationViewModel(FinanceReconciliationViewModel viewModel)
		{

			if (viewModel == null)
			{
				viewModel = new FinanceReconciliationViewModel();
			}

			try
			{
				viewModel = (FinanceReconciliationViewModel)InitDropDown(viewModel);
				if (_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo)
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

		public IList<FinanceOrder> GetFinanceOrderList(FinanceTableDataModel financeTableDataModel)
		{
			var result = _customersServices.GetFinanceOrderList(financeTableDataModel);

			TableDataModel tableDataModel = new TableDataModel
			{
				Limit = 0,
				Offset = 0,
				Order = financeTableDataModel.Order,
				Search = financeTableDataModel.Search,
				Sort = financeTableDataModel.Sort
			};
			result = GetSortAndSearchList(result.AsQueryable(), tableDataModel).ToList();

			return result;
		}


		public List<FinanceDayCashFlow> GetFinanceDayCashFlowList(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			List<FinanceDayCashFlow> result = new List<FinanceDayCashFlow>();

			result = _customersServices.GetFinanceCashFlowList(contrAgentCode, startDate, endDate);


			return result;
		}

		public FileContentResult CashFlowGenerateFile(List<FinanceDayCashFlow> financeDayCashFlowList)
		{
			StringBuilder sb = new StringBuilder();



			sb.AppendLine(string.Format("{0};{1};{2};{3};{4}",
				Resources.Finance.Date,
				Resources.Finance.OrderNumber,
				Resources.Finance.Income,
				Resources.Finance.Outcome,
				Resources.Finance.Balance

			));

			foreach (var day in financeDayCashFlowList)
			{
				sb.AppendLine(string.Format("{0};{1};{2:F2};{3};{4}",
					day.Day.Date,
					"", day.TotalOutcome,
					"",
					""

				));
				foreach (var order in day.FinanceCashFlow)
				{
					sb.AppendLine(string.Format("{0};{1};{2};{3:F2};{4}",
						"",
						order.OrderNumber,
						"", order.OutcomeValue,
						""
					));
				}
				sb.AppendLine(string.Format("{0};{1};{2:F2};{3:F2};{4:F2}",
					Resources.Finance.Total,
					"", day.TotalIncome, day.TotalOutcome, day.TotalStartIncomeEnd));
			}


			var fileData = Encoding.UTF8.GetBytes(sb.ToString());
			var bom = new byte[] { 0xEF, 0xBB, 0xBF };
			var csvData = new byte[bom.Length + fileData.Length];
			Array.Copy(bom, csvData, bom.Length);
			Array.Copy(fileData, 0, csvData, bom.Length, fileData.Length);
			string filename = $"Export-{DateTime.Now:yyyyMMdd-HHmmss}.csv";
			var file = new FileContentResult(csvData, "text/csv")
			{
				FileDownloadName = filename
			};
			return file;
		}


		public FinanceReconciliationList GetFinanceReconciliationList(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			FinanceReconciliationList result = new FinanceReconciliationList();

			var dto = _customersServices.GetFinanceReconciliationList(contrAgentCode, startDate, endDate);
			result.FinanceAdvanceReconciliationList = dto.FinanceAdvanceReconciliationList;
			result.FinanceMainReconciliationList = dto.FinanceMainReconciliationList;
			result.FullBeginTotal = dto.FullBeginTotal;
			result.FullIncomeTotal = dto.FullIncomeTotal;
			result.FullOutcomeTotal = dto.FullOutcomeTotal;
			result.FullEndTotal = dto.FullEndTotal;



			return result;
		}

		public FileContentResult ReconciliationGenerateFile(FinanceReconciliationList financeReconciliationList)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5}",
			   Resources.Finance.Document,
				Resources.Finance.Orders,
				Resources.Finance.InitialBalance,
				Resources.Finance.Income,
				Resources.Finance.Outcome,
				Resources.Finance.FinalBalance
				));

			sb.AppendLine(string.Format("{0};{1};{2:F2};{3:F2};{4:F2};{5:F2}",
				financeReconciliationList.FinanceAdvanceReconciliationList.Document,
				"",
				financeReconciliationList.FinanceAdvanceReconciliationList.InitialBalance,
				financeReconciliationList.FinanceAdvanceReconciliationList.IncomeValue,
				financeReconciliationList.FinanceAdvanceReconciliationList.OutcomeValue,
				financeReconciliationList.FinanceAdvanceReconciliationList.FinalBalance
				));



			foreach (var order in financeReconciliationList.FinanceMainReconciliationList)
			{
				sb.AppendLine(string.Format("{0};{1};{2:F2};{3:F2};{4:F2};{5:F2}",
					order.Document,
					"",
					order.TotalInitialBalance,
					order.TotalIncomeValue,
					order.TotalOutcomeValue,
					order.TotalFinalBalance
					));


				foreach (var item in order.ReconciliationList)
				{
					sb.AppendLine(string.Format("{0};{1};{2:F2};{3:F2};{4:F2};{5:F2}",
						"",
						item.OrderNumber,
						item.InitialBalance,
						item.IncomeValue,
						item.OutcomeValue,
						item.FinalBalance
						));
				}
			}


			var fileData = Encoding.UTF8.GetBytes(sb.ToString());
			var bom = new byte[] { 0xEF, 0xBB, 0xBF };
			var csvData = new byte[bom.Length + fileData.Length];
			Array.Copy(bom, csvData, bom.Length);
			Array.Copy(fileData, 0, csvData, bom.Length, fileData.Length);
			string filename = string.Format("Export-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
			var file = new FileContentResult(csvData, "text/csv")
			{
				FileDownloadName = filename
			};
			return file;

		}

		public OrdersPaymentsViewModel PrepareOrdersPaymentsViewModel(OrdersPaymentsViewModel viewModel = null)
		{
			if (viewModel == null)
			{
				viewModel = new OrdersPaymentsViewModel();
			}

			viewModel.StartDate = DateTime.Now.AddMonths(-1);
			viewModel.EndDate = DateTime.Now;

			if (_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo)
			{
				viewModel.ContrAgentCode = _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();
			}

			return viewModel;
		}

		public Create1COrderViewModel GetCustomerBillValues()
		{
			Create1COrderViewModel viewModel = new Create1COrderViewModel();
			if (_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo)
			{
				var contrCode = _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();
				var model = _customersServices.GetCustomerBill(contrCode);

				viewModel.Ibans = model.IbanList;
				viewModel.Addresses = model.AddressList;
				viewModel.Contragents = model.ContragentList;
				viewModel.Items = model.ItemList;
			}

			return viewModel;
		}

		public bool CreateCustomerBill(Create1COrderViewModel model)
		{

			if (_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo)
			{
				var contrCode = _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();
				_customersServices.CreateCustomerBill(model, contrCode);
			}

			return true;
		}

		public CustomerBillListViewModel GetCustomerBillList(CustomerBillListViewModel viewModel)
		{
			if (viewModel == null)
			{
				viewModel = new CustomerBillListViewModel();
			}

			viewModel = (CustomerBillListViewModel)InitDropDown(viewModel);

			if ((_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo) || !_applicationContext.isCustomer)
			{
				var contrCode = (!string.IsNullOrEmpty(viewModel.ContrAgentCode)) ? viewModel.ContrAgentCode : _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();

				List<CustomerBillListDto> list = _customersServices.GetCustomerBillList1c(contrCode);

				viewModel.CustomerBillList = new List<CustomerBillViewModel>();

				foreach (var item in list.Where(x => x.Date >= viewModel.StartDate && x.Date <= viewModel.EndDate.AddDays(1).AddTicks(-1)))
				{
					viewModel.CustomerBillList.Add(new CustomerBillViewModel
					{
						Guid = item.Guid,
						Date = item.Date,
						Suma = item.Suma,
						Number = item.Number
					});
				}
			}

			return viewModel;
		}

		public WdsActionCodeViewModel PrepareWdsActionViewModel()
		{
			var contrCode = _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();

			WdsActionCodeViewModel model = new WdsActionCodeViewModel
			{
				DateStart = _initWdsDate,
				DateEnd = DateTime.Now,
				ContrCode = contrCode
			};

			model.ContrAgentCodeList = new List<SelectListItem>();

			model.ContrAgentCodeList.AddRange(_customersServices.GetCustomerCodesList()
																	.OrderBy(x => x.CustomerName)
																	.Select(v => new SelectListItem
																	{
																		Text = v.CustomerName.ToString(),
																		Value = v.Code.ToString()
																	}).ToList());

			model.ContrAgentCodeList.Insert(0, new SelectListItem { Value = "", Text = "Оберіть контрагента" });

			return model;
		}

		public WdsActionCodeViewModel GetWdsCodes(WdsActionCodeViewModel model)
		{



			var wdsCodes = _customersServices.GetWdsActionCodes(model.ContrCode, model.DateStart, model.DateEnd);
			model.WdsActionCodeList = wdsCodes;

			model.ContrAgentCodeList = new List<SelectListItem>();

			model.ContrAgentCodeList.AddRange(_customersServices.GetCustomerCodesList()
																.OrderBy(x => x.CustomerName)
																.Select(v => new SelectListItem
																{
																	Text = v.CustomerName.ToString(),
																	Value = v.Code.ToString()
																}).ToList());

			model.ContrAgentCodeList.Insert(0, new SelectListItem { Value = "", Text = "Оберіть контрагента" });

			return model;

		}
		public string GetCustomerBillByGuid(string guid)
		{
			var pdfDocument = String.Empty;

			if ((_applicationContext.isCustomer && _applicationContext.PermitFinanceInfo) || !_applicationContext.isCustomer)
			{

				pdfDocument = _customersServices.GetCustomerBillByGuid(guid);
			}

			return pdfDocument;

		}
		public OrdersPaymentsViewModel GetGetOrderPayments(OrdersPaymentsViewModel viewModel)
		{
			if (viewModel == null)
			{
				viewModel = new OrdersPaymentsViewModel
				{
					StartDate = DateTime.Now.AddMonths(-1),
					EndDate = DateTime.Now
				};
			}

			if (_applicationContext.isCustomer)
			{
				var contrCode = _accountRepository.GetByIdIncludes(_applicationContext.AccountId).Customer?.CustomerContrCode.ToString();

				var financeTableDataModel = new FinanceTableDataModel
				{
					ContrAgentCode = contrCode,
					EndDate = viewModel.EndDate,
					StartDate = viewModel.StartDate
				};

				viewModel.AvailableAdvance = (decimal)_webOrderServices.GetCustomerAdvance(contrCode);

				// 
				viewModel.Payments = _customersServices.GetFinanceOrderList(financeTableDataModel, true).OrderBy(x => x.StartDate.Value).Where(x => x.AdvanceValue < x.TotalValue).Select(n => new OrderPayment
				{
					OrderDate = n.StartDate,
					OrderNumber = n.Number,
					CurrentAdvance = n.AdvanceValue,
					Payment = 0,
					Price = n.TotalValue,
					Status = n.Status,
					Agreement = n.Agreement
				}).OrderByDescending(s => s.Status).ToList();

				viewModel.ContrAgentCode = contrCode;

			}


			return viewModel;
		}

		public bool ProceedOrdersPayments(OrdersPaymentsViewModel model)
		{
			try
			{
				if (_webOrderServices.ProceedPayments(model.Payments.Where(x => x.Payment > 0).Select(x => new PaymentDocumentDataDto
				{
					OrderNumber = x.OrderNumber,
					PaymentSum = (double)x.Payment
				}).ToList()))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}
			return false;
		}

		public List<WdsActionRatingListViewModel> GetWdsActionRating(int topNumber, int daysInWdsRating)
		{

			List<WdsActionRatingListViewModel> viewModel = new List<WdsActionRatingListViewModel>();
			DateTime endDate = DateTime.Today.AddDays(1);
			//DateTime endDate = new DateTime(2024, 5, 1);
			//DateTime startDate = DateTime.Today.AddDays(-daysInWdsRating);
			DateTime startDate = endDate.AddDays(-daysInWdsRating);

			var data = _customersServices.GetWdsActionCustomerRating(startDate, endDate, topNumber);

			if (data.Count > 0)
			{
				var maxOrder = data[0].RatingSum;
				foreach (var item in data)
				{

					WdsActionRatingListViewModel modelItem = new WdsActionRatingListViewModel();
					modelItem.CustomerContrCode = Int32.Parse(item.CustomerContrCode);

					var customer = _accountRepository.GetCustomerByContrCode(Int32.Parse(item.CustomerContrCode));

					modelItem.CustomerName = customer != null ? customer.OrderPortalUser.FullName : $"не визначено: {item.CustomerContrCode}";
					//	_accountRepository.GetCustomerByContrCode(Int32.Parse(item.CustometContrCode))?.OrderPortalUser.FullName;
					modelItem.RatingSum = item.RatingSum;
					modelItem.RatingValue = Math.Round(item.RatingSum / maxOrder * 100, 0);

					viewModel.Add(modelItem);

				}
			}

			return viewModel;

		}
		//=================================================================
		#region Private Func

		private FinanceBaseViewModel InitDropDown(FinanceBaseViewModel viewModel)
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

		public IQueryable<T> GetSortAndSearchList<T>(IQueryable<T> list, TableDataModel tableDataModel) where T : FinanceOrder
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
