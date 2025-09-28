using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Dto.Customer1cOrder;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Models.Analytics;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;
using OrdersPortal.Services.WebPortal1cService;
using OrdersPortal.Services.WebPortalComplaints1cService;
using OrdersPortal.Services.WebPortalOrderBillService;

using Unity.Injection;

namespace OrdersPortal.Services.Services
{
	public class CustomersServices : ICustomersServices
	{
		private readonly IAccountRepository _accountRepository;
		private readonly complaintsPortTypeClient _complaintService;
		private readonly cportal_vsPortTypeClient _service;
		private readonly cportal_bill_vsPortTypeClient _orderBillService;


		//#if DEBUG
		//		private const string ServiceEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/cportal_vs";
		//		private const string ComplaintsEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/ws_complaints.1cws";
		//		private const string OrderBillEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/cportal_bill_vs";
		//#else
		//     	private const string ServiceEndPointAddress = "http://192.168.1.10/oknastyle/ws/cportal_vs";
		//		private const string ComplaintsEndPointAddress = "http://192.168.1.10/oknastyle/ws/ws_complaints.1cws";
		//		private const string OrderBillEndPointAddress = "http://192.168.1.10/oknastyle/ws/cportal_bill_vs";

		//#endif


		//private const string ServiceEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/cportal_vs";
		//private const string ComplaintsEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/ws_complaints.1cws";
		//private const string OrderBillEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/cportal_bill_vs";

		private const string ServiceEndPointAddress = "http://192.168.1.10/oknastyle/ws/cportal_vs";
		private const string ComplaintsEndPointAddress = "http://192.168.1.10/oknastyle/ws/ws_complaints.1cws";
		private const string OrderBillEndPointAddress = "http://192.168.1.10/oknastyle/ws/cportal_bill_vs";


		public CustomersServices(IAccountRepository accountRepository)
		{

			_accountRepository = accountRepository;

			BasicHttpBinding myBinding = new BasicHttpBinding();

			myBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
			myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
			myBinding.MaxReceivedMessageSize = 2147483647;
			myBinding.MaxBufferSize = 2147483647;
			myBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);


			EndpointAddress complaintEa = new EndpointAddress(@ComplaintsEndPointAddress);
			_complaintService = new complaintsPortTypeClient(myBinding, complaintEa);

			_complaintService.ClientCredentials.UserName.UserName = "webuser";
			_complaintService.ClientCredentials.UserName.Password = "webP@ssw0rd2014";


			EndpointAddress ea = new EndpointAddress(@ServiceEndPointAddress);
			_service = new cportal_vsPortTypeClient(myBinding, ea);

			_service.ClientCredentials.UserName.UserName = "webuser";
			_service.ClientCredentials.UserName.Password = "webP@ssw0rd2014";


			EndpointAddress orderBillEa = new EndpointAddress(OrderBillEndPointAddress);
			_orderBillService = new cportal_bill_vsPortTypeClient(myBinding, orderBillEa);

			_orderBillService.ClientCredentials.UserName.UserName = "webuser";
			_orderBillService.ClientCredentials.UserName.Password = "webP@ssw0rd2014";
		}





		public bool CheckOrderInPeriodService(int? kod, string orderNum, DateTime startDate)
		{
			bool checkResult = false;
			if (kod != null)
			{
				string contrCode = CustomerHelper.GetContrAgentFullCode(kod.ToString());
				var res = _complaintService.check_kontr_order(orderNum, contrCode, startDate);

				checkResult = res;

			}
			return checkResult;
		}

		public bool Set1CDeliveryStatus(string kod, string orderNum, string org1cCode = "000000001")
		{
			var res = _service.set_order_state_wait_for_pay_by_org(orderNum, kod, org1cCode);

			bool checkResult = res > 0;

			return checkResult;
		}

		public bool Set1COrderPartsDeliveryStatus(string kod, int orderPartsId)
		{
			var res = _service.set_orderparts_state(orderPartsId, kod);

			bool checkResult = res > 0;

			return checkResult;
		}

		public List<Manager1c> GetManagerList(string org1cId)
		{
			List<Manager1c> result = new List<Manager1c>();

			try
			{
				var res = _service.GetManagers_by_organizations(org1cId);

				if (res != null)
				{
					foreach (var item in res)
					{
						result.Add(new Manager1c { Guid = item._1c_ID, Name = item._1c_name });
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result.OrderBy(x => x.Name).ToList();
		}

		public List<RegionManager1c> GetRegionManagerList(string org1cId)
		{
			List<RegionManager1c> result = new List<RegionManager1c>();


			// Получаем список контрагентов методом "GetList"

			try
			{
				var res = _service.GetRegionalManagers_by_organizations(org1cId);

				if (res != null)
				{
					foreach (var item in res)
					{
						result.Add(new RegionManager1c { Guid = item._1c_ID, Name = item._1c_name });
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result.OrderBy(x => x.Name).ToList();
		}
		public List<Customer1c> GetManagerCustomerList(string managerGiud, Boolean isRegionManager = false)
		{
			List<Customer1c> result = new List<Customer1c>();




			try
			{
				var res = !isRegionManager ? _service.get_manager_kontr_list(managerGiud) : _service.get_manager_kontr_list(managerGiud);

				if (res != null)
				{
					foreach (var item in res)
					{
						result.Add(new Customer1c { Name = (string)item.name, Code = (string)item.kod, Manager1cGuid = managerGiud });
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;
		}

		public bool UpdateManagers(Update1cManagersDto model)
		{
			bool result = false;

			List<Customer> customersList = new List<Customer>();
			try
			{
				if (model.Managers)
				{
					var managers1c = GetManagerList(model.Organization1cId);
					foreach (var manager in managers1c)
					{
						var managerUser = _accountRepository.GetManagerByGuid(manager.Guid);
						if (managerUser != null)
						{
							var customers = GetManagerCustomerList(manager.Guid);
							foreach (var customer in customers)
							{
								int contrCode = CustomerHelper.GetContrAgentShortCode(customer.Code);
								if (contrCode == 11620)
								{
									contrCode = contrCode;
								}
								var portalCustomer = _accountRepository.GetCustomerByContrCode(contrCode);
								if (portalCustomer != null)
								{
									portalCustomer.ManagerId = managerUser.OrderPortalUser.Id;
									customersList.Add(portalCustomer);

								}
							}
						}

					}

					_accountRepository.SaveChanges();
				}

				if (model.RegionManagers)
				{

					var regionManagers1c = GetRegionManagerList(model.Organization1cId);
					foreach (var regionManager in regionManagers1c)
					{
						var regionManagerUser = _accountRepository.GetRegionManagerByGuid(regionManager.Guid);
						if (regionManagerUser != null)
						{
							var customers = GetManagerCustomerList(regionManager.Guid, true);
							foreach (var customer in customers)
							{
								int contrCode = CustomerHelper.GetContrAgentShortCode(customer.Code);
								var portalCustomer = _accountRepository.GetCustomerByContrCode(contrCode);
								if (portalCustomer != null)
								{
									portalCustomer.RegionManagerId = regionManagerUser.OrderPortalUser.Id;
									customersList.Add(portalCustomer);

								}
							}
						}

					}

					_accountRepository.SaveChanges();
				}

				model.UpdatedCustomers = customersList;
				result = true;
			}
			catch (Exception ex)
			{
				ex = ex;
				result = false;
			}

			return result;
		}

		public List<ComplaintOrderSerieDto> GetComplaintOrderSeriesByOrderNumber(string orderNumber, int orderId)
		{
			List<ComplaintOrderSerieDto> result = new List<ComplaintOrderSerieDto>();
			try
			{
				var res = _complaintService.getComplaintSeries(orderNumber);

				foreach (var item in res)
				{
					result.Add(new ComplaintOrderSerieDto
					{
						SerieGuid = item.GUID,
						SerieName = item.Name,
						SerieCategory = item.ParentName,
						OrderId = orderId
					});

				}

			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;

		}

		public List<ComplaintIssueDto> LoadIssueAndSolutionList()
		{

			var result = _complaintService.getComplaintIssuesList().Select(x => new ComplaintIssueDto
			{
				Name = x.Name,
				Guid = x.GUID

			}).ToList();

			return result;

		}

		public List<ComplaintIssueDto> GetComplaintSolutionsListByIssue(string issueGuid)
		{
			var res = _complaintService.getComplaintSolutionsListByIssue(issueGuid).Select(x => new ComplaintIssueDto
			{
				Name = x.Name,
				Guid = x.GUID

			}).ToList(); ;
			return res;
		}
		public List<CustomerCodesListDto> GetCustomerCodesList(string org1cCode = "000000001")
		{
			List<CustomerCodesListDto> contrAgentsList = new List<CustomerCodesListDto>();
			try
			{
				if (contrAgentsList.Count < 1)
				{
					// todo - change to proper org
					var res = _service.get_kontr_list_by_organizations(org1cCode);

					if (res != null)
					{
						foreach (var cn in res)
						{
							contrAgentsList.Add(new CustomerCodesListDto
							{
								CustomerName = cn.name.ToString(),
								Code = Convert.ToInt32(cn.kod)
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return contrAgentsList;
		}

		public List<CustomerCodesListDto> GetCustomerCodesListByOrganization(string org1cId)
		{
			List<CustomerCodesListDto> contrAgentsList = new List<CustomerCodesListDto>();
			try
			{
				if (contrAgentsList.Count < 1)
				{
					var res = _service.get_kontr_list_by_organizations(org1cId);

					if (res != null)
					{
						foreach (var cn in res)
						{
							contrAgentsList.Add(new CustomerCodesListDto
							{
								CustomerName = cn.name.ToString(),
								Code = Convert.ToInt32(cn.kod)
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return contrAgentsList;
		}


		public IList<FinanceOrder> GetFinanceOrderList(FinanceTableDataModel financeTableDataModel, bool isPayment = false)
		{
			List<FinanceOrder> result = new List<FinanceOrder>();

			DateTime startReportDate = financeTableDataModel.StartDate ?? new DateTime(DateTime.Now.Year, 1, 1);
			DateTime endReportDate = financeTableDataModel.EndDate ?? DateTime.Now;

			try
			{

				var importList = _service.get_order_list(CustomerHelper.GetContrAgentFullCode(financeTableDataModel.ContrAgentCode), startReportDate, endReportDate, isPayment);

				if (importList != null && importList.Count > 0)
				{
					foreach (var item in importList)
					{
						int statusId = 0;

						if (item.product_status.IndexOf("В роботі") == 0)
						{
							statusId = 1;
						}
						else if (item.product_status.IndexOf("На складі") == 0)
						{
							statusId = 2;
						}
						else
						{
							statusId = 3;
						}
						FinanceOrder order = new FinanceOrder
						{
							AdvanceValue = (decimal)item.pay_sum,
							BalanceValue = (decimal)item.debt_sum,
							DelayDate = item.delay_date > new DateTime(1, 1, 2) ? item.delay_date.ToShortDateString() : "",
							DeliveryDate = item.delivery_date > new DateTime(1, 1, 2) ? item.delivery_date.ToShortDateString() : "",
							Number = item.order_number,
							QuantityConstructions = (int)item.constr_sum,
							ReadyDate = item.date_ready != String.Empty ? item.date_ready : "",
							ShipmentDate = item.delivery_date > new DateTime(1, 1, 2) ? item.delivery_date.ToShortDateString() : "",
							StartDate = item.date_start > new DateTime(1, 1, 2) ? item.date_start : (DateTime?)null,
							TotalValue = (decimal)item.order_sum,
							Status = statusId.ToString(),
							Agreement = item.contract_name
						};


						result.Add(order);
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;

		}

		public IList<AnalyticsOrder> GetAnalyticOrderList(AnalyticsTableDataModel analyticsTableDataModel, bool isPayment = false)
		{
			List<AnalyticsOrder> result = new List<AnalyticsOrder>();

			DateTime startReportDate = analyticsTableDataModel.StartDate ?? new DateTime(DateTime.Now.Year, 1, 1);
			DateTime endReportDate = analyticsTableDataModel.EndDate ?? DateTime.Now;

			try
			{

				var importList = _service.get_order_list(CustomerHelper.GetContrAgentFullCode(analyticsTableDataModel.ContrAgentCode), startReportDate, endReportDate, isPayment);

				if (importList != null && importList.Count > 0)
				{
					foreach (var item in importList)
					{
						int statusId = 0;

						if (item.product_status.IndexOf("В роботі") == 0)
						{
							statusId = 1;
						}
						else if (item.product_status.IndexOf("На складі") == 0)
						{
							statusId = 2;
						}
						else
						{
							statusId = 3;
						}
						AnalyticsOrder order = new AnalyticsOrder
						{
							AdvanceValue = (decimal)item.pay_sum,
							BalanceValue = (decimal)item.debt_sum,
							DelayDate = item.delay_date > new DateTime(1, 1, 2) ? item.delay_date.ToShortDateString() : "",
							DeliveryDate = item.delivery_date > new DateTime(1, 1, 2) ? item.delivery_date.ToShortDateString() : "",
							Number = item.order_number,
							QuantityConstructions = (int)item.constr_sum,
							ReadyDate = item.date_ready != String.Empty ? item.date_ready : "",
							ShipmentDate = item.delivery_date > new DateTime(1, 1, 2) ? item.delivery_date.ToShortDateString() : "",
							StartDate = item.date_start > new DateTime(1, 1, 2) ? item.date_start : (DateTime?)null,
							TotalValue = (decimal)item.order_sum,
							Status = statusId.ToString(),
							Agreement = item.contract_name,
							Category = item.prod_days_group,
							ActualyProductionDays = 0,//(int)item.prod_days,
							ProfileSystem = item.profile_system
						};


						result.Add(order);
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;

		}
		public List<AnalyticsCategoryOrder> GetOrderAnalyticsList(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			List<AnalyticsCategoryOrder> result = new List<AnalyticsCategoryOrder>();


			try
			{

				var serviceResult = _service.get_order_list(CustomerHelper.GetContrAgentFullCode(CustomerHelper.GetContrAgentFullCode(contrAgentCode)), startDate, endDate, false);
				if (serviceResult != null)
				{
					var categories = serviceResult.GroupBy(x => x.prod_days_group).Select(x => x.Key).ToList();

					foreach (var category in categories)
					{
						AnalyticsCategoryOrder analyticsCategory = new AnalyticsCategoryOrder()
						{

							Category = category,
							AnalyticsOrderList = new List<AnalyticsOrder>()
						};



						foreach (var item in serviceResult.Where(x => x.prod_days_group == category).ToList())
						{


							if (!string.IsNullOrEmpty(item.order_number))
							{

								AnalyticsOrder analyticsOrder = new AnalyticsOrder()
								{
									AdvanceValue = (decimal)item.pay_sum,
									BalanceValue = (decimal)item.debt_sum,
									DelayDate = item.delay_date > new DateTime(1, 1, 2) ? item.delay_date.ToShortDateString() : "",
									DeliveryDate = item.delivery_date > new DateTime(1, 1, 2) ? item.delivery_date.ToShortDateString() : "",
									Number = item.order_number,
									QuantityConstructions = (int)item.constr_sum,
									ReadyDate = item.date_ready != String.Empty ? item.date_ready : "",
									ShipmentDate = item.delivery_date > new DateTime(1, 1, 2) ? item.delivery_date.ToShortDateString() : "",
									StartDate = item.date_start > new DateTime(1, 1, 2) ? item.date_start : (DateTime?)null,
									TotalValue = (decimal)item.order_sum,
									Status = item.product_status,
									Agreement = item.contract_name,
									Category = item.prod_days_group,
									ActualyProductionDays = 0, // (int)item.prod_days,
									ProfileSystem = item.profile_system

								};

								analyticsCategory.AnalyticsOrderList.Add(analyticsOrder);
							}

						}

						result.Add(analyticsCategory);
					}

				}

			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;
		}

		public List<AnalyticsProfileSystem> GetAnalyticsProfileSystem(string contrAgentCode, DateTime startDate, DateTime endDate)
		{

			List<AnalyticsProfileSystem> result = new List<AnalyticsProfileSystem>();

			var serviceResult = _service.get_order_list(CustomerHelper.GetContrAgentFullCode(CustomerHelper.GetContrAgentFullCode(contrAgentCode)), startDate, endDate, false);
			if (serviceResult != null)
			{
				var profileSystems = serviceResult.GroupBy(x => x.profile_system).Select(x => x.Key).ToList();

				foreach (var profile in profileSystems)
				{
					var sum = serviceResult.Where(x => x.profile_system == profile).Sum(x => x.constr_sum);
					var avarageBill = serviceResult.Where(x => x.profile_system == profile).Sum(x => x.order_sum) / serviceResult.Where(x => x.profile_system == profile).Sum(x => x.constr_sum);
					if (sum > 0)
					{
						AnalyticsProfileSystem analyticsProfileSystem = new AnalyticsProfileSystem()
						{
							ProfileSystemName = profile.Replace("\"", "'"),
							ProfileSystemValue = (int)Math.Round(sum),
							ProfileSystemAvarageBill = Math.Round(avarageBill,2)

						};

						result.Add(analyticsProfileSystem);
					}
				}
			}
			return result;
		}

		public List<FinanceDayCashFlow> GetFinanceCashFlowList(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			List<FinanceDayCashFlow> result = new List<FinanceDayCashFlow>();
			
			try
			{
				var serviceResult = _service.get_debt_movement2019(CustomerHelper.GetContrAgentFullCode(contrAgentCode), startDate, endDate);
				if (serviceResult != null)
				{
					var groupedDays = serviceResult.GroupBy(x => x.order_date).Select(x => x.Key).ToList();

					foreach (var resDay in groupedDays)
					{
						FinanceDayCashFlow financeDayCashFlow = new FinanceDayCashFlow();
						financeDayCashFlow.FinanceCashFlow = new List<FinanceCashFlow>();

						financeDayCashFlow.Day = resDay ?? DateTime.Now;  //todo change on correct value


						decimal total = 0;
						decimal totalIncome = 0;
						decimal totalStartIncome = 0;


						foreach (var item in serviceResult.Where(x => x.order_date == resDay).ToList())
						{


							if (!string.IsNullOrEmpty(item.order_number) && item.order_number != "Підсумок за день")
							{
								FinanceCashFlow orderMoneyMovement = new FinanceCashFlow()
								{
									Document = item.contract,
									OrderNumber = item.order_number,
									OutcomeValue = (decimal)item.Summ_Out,
									IncomeValue = (decimal)item.Summ_In,
									BeginPeriod = (decimal)item.Summ_Start,
									EndPeriod = item.Summ_End
								};
								financeDayCashFlow.FinanceCashFlow.Add(orderMoneyMovement);
							}


							if (string.IsNullOrEmpty(item.order_number))
								financeDayCashFlow.TotalStartIncome = (decimal)item.Summ_Start;

							if (item.order_number == "Підсумок за день")
							{
								financeDayCashFlow.TotalIncome = (decimal)item.Summ_In;
								financeDayCashFlow.TotalOutcome = (decimal)item.Summ_Out;
								financeDayCashFlow.TotalStartIncomeEnd = (decimal)item.Summ_Start;

								financeDayCashFlow.RestIncome = item.Summ_End;
							}
						
						}						

						result.Add(financeDayCashFlow);
					}
				}

			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;
		}

		public FinanceReconciliationListDto GetFinanceReconciliationList(string contrAgentCode, DateTime startDate, DateTime endDate)
		{
			FinanceReconciliationListDto result = new FinanceReconciliationListDto();
			result.FinanceMainReconciliationList = new List<FinanceMainReconciliation>();
			result.FinanceAdvanceReconciliationList = new FinanceReconciliation();

			try
			{


				var reconciliation = _service.get_debt_movement(CustomerHelper.GetContrAgentFullCode(contrAgentCode), startDate, endDate);
				foreach (var item in reconciliation)
				{
					if (item.is_main_contract == 0)
					{
						var advanceReconciliation = new FinanceReconciliation
						{
							Document = item.contract,
							InitialBalance = (decimal)item.Summ_Start,
							IncomeValue = (decimal)item.Summ_In,
							OutcomeValue = (decimal)item.Summ_Out,
							FinalBalance = (decimal)item.Summ_End
						};
						result.FinanceAdvanceReconciliationList = advanceReconciliation;

					}
				}

				foreach (var item in reconciliation.Where(x => x.is_main_contract == 1).GroupBy(x => x.order_status).Select(x => x.Key))
				{
					FinanceMainReconciliation mainReconciliationList = new FinanceMainReconciliation
					{
						Document = item,
						ReconciliationList = new List<FinanceReconciliation>(),
						TotalOutcomeValue = 0,
						TotalIncomeValue = 0,
						TotalFinalBalance = 0,
						TotalInitialBalance = 0

					};
					foreach (var order in reconciliation.Where(x => x.order_status == item && x.is_main_contract == 1 && (x.Summ_In != 0 || x.Summ_Out != 0)))
				
					{
						FinanceReconciliation orderMoneyMovement = new FinanceReconciliation
						{
							OrderNumber = order.order_number,
							InitialBalance = (decimal)order.Summ_Start,
							IncomeValue = (decimal)order.Summ_In,
							OutcomeValue = (decimal)order.Summ_Out,
							FinalBalance = (decimal)order.Summ_End
						};
						mainReconciliationList.ReconciliationList.Add(orderMoneyMovement);
						mainReconciliationList.TotalIncomeValue += (decimal)order.Summ_In;
						mainReconciliationList.TotalOutcomeValue += (decimal)order.Summ_Out;
						mainReconciliationList.TotalIncomeValue += (decimal)order.Summ_Start;
						mainReconciliationList.TotalFinalBalance += (decimal)order.Summ_End;


						result.FullIncomeTotal += (decimal)order.Summ_In;
						result.FullOutcomeTotal += (decimal)order.Summ_Out;
						result.FullBeginTotal += (decimal)order.Summ_Start;
						result.FullEndTotal += (decimal)order.Summ_End;

					}
					result.FinanceMainReconciliationList.Add(mainReconciliationList);
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}


			return result;

		}

		public Customer1cOrderValues GetCustomerBill(string contrCode)
		{
			var result = new Customer1cOrderValues();
			try
			{
				if (!String.IsNullOrEmpty(contrCode))
				{
					var res = _orderBillService.get_customer_bill_info(CustomerHelper.GetContrAgentFullCode(contrCode));

					if (res != null)
					{
						var contrAgents = new List<CustomerOrderContragent>();

						foreach (var cn in res.Where(x => x.TYPE == "Contragent").ToList())
						{
							contrAgents.Add(new CustomerOrderContragent
							{
								Guid = cn.GUID,
								Name = cn.NAME
							});
						}

						result.ContragentList = contrAgents;

						var ibans = new List<CustomerOrderIban>();

						foreach (var cn in res.Where(x => x.TYPE == "Iban").ToList())
						{
							ibans.Add(new CustomerOrderIban
							{
								Guid = cn.GUID,
								Name = cn.NAME
							});
						}

						result.IbanList = ibans;


						var addresses = new List<CustomerOrderAddress>();

						foreach (var cn in res.Where(x => x.TYPE == "Address").ToList())
						{
							addresses.Add(new CustomerOrderAddress
							{
								Guid = cn.GUID,
								Name = cn.NAME
							});
						}

						result.AddressList = addresses;


						var items = new List<CustomerOrderItem>();

						foreach (var cn in res.Where(x => x.TYPE == "Item").ToList())
						{
							items.Add(new CustomerOrderItem
							{
								Guid = cn.GUID,
								Name = cn.NAME
							});
						}

						result.ItemList = items;
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result;
		}

		public bool CreateCustomerBill(Domain.Dto.Customer1cOrder.Create1COrder model, string contrCode)
		{
			var dto = new WebPortalOrderBillService.Create1COrder();

			dto.Address = model.Address;
			dto.InternalComment = model.InternalComment;
			dto.OrderContrAgent = model.OrderContrAgent;
			dto.OrderCreateDate = Convert.ToDateTime(model.OrderCreateDate);
			dto.OrderDeliveryDate = Convert.ToDateTime(model.OrderDeliveryDate);
			dto.OrderIban = model.OrderIban;
			dto.OrderNumber = model.OrderNumber;
			dto.OrderSuma = Convert.ToDouble(!String.IsNullOrEmpty(model.OrderSuma) ? model.OrderSuma.Replace('.', ',') : "0");



			OrderItemsDATA[] itemArray = new OrderItemsDATA[model.OrderItems.Count];

			for (int i = 0; i < model.OrderItems.Count; i++)
			{
				itemArray[i] = new OrderItemsDATA
				{
					Count = Convert.ToDouble(model.OrderItems[i].Count.Replace('.', ',')),
					Price = Convert.ToDouble(model.OrderItems[i].Price.Replace('.', ',')),
					Height = model.OrderItems[i].Height,
					Width = model.OrderItems[i].Width,
					ItemGUID = model.OrderItems[i].OrderItem.Guid

				};
			}

			dto.OrderItemsLIST = itemArray;
			try
			{
				var result = _orderBillService.set_customer_bill(dto);
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return true;
		}

		public List<CustomerBillListDto> GetCustomerBillList1c(string contrCode)
		{
			List<CustomerBillListDto> list = new List<CustomerBillListDto>();
			try
			{
				if (!String.IsNullOrEmpty(contrCode))
				{
					var res = _orderBillService.get_customer_bill_list(CustomerHelper.GetContrAgentFullCode(contrCode));

					if (res != null)
					{
						foreach (var item in res)
						{
							list.Add(new CustomerBillListDto
							{
								Guid = item.GUID,
								Number = item.number,
								Suma = (decimal)item.bill_sum,
								Date = item.date
							});
						}

					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return list;
		}

		public string GetCustomerBillByGuid(string guid)
		{
			string pdfDocument = String.Empty;
			try
			{
				if (!String.IsNullOrEmpty(guid))
				{
					var res = _orderBillService.get_customer_bill_PDF(guid);

					if (res != null)
					{
						pdfDocument = res;

					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return pdfDocument;
		}

		public List<WdsActionCodeListModel> GetWdsActionCodes(string contrAgentCode, DateTime startDate, DateTime endDate)
		{

			try
			{
				List<WdsActionCodeListModel> model = new List<WdsActionCodeListModel>();

				var orders = _service.get_order_list(CustomerHelper.GetContrAgentFullCode(contrAgentCode), startDate, endDate, false);

				foreach (var order in orders)
				{
					var res = _service.get_wds(order.order_number.Replace(" ", ""));
					foreach (var item in res)
					{


						model.Add(new WdsActionCodeListModel { Db1cOrderNumber = order.order_number, Db1cOrderSerie = item.seria_number, Db1cWdsActionCode = item.kod_wds });
					}
				}

				return model;
			}
			catch (Exception ex)
			{
				ex = ex;
			}


			return null;
		}

		public List<WdsActionRatingListModel> GetWdsActionCustomerRating(DateTime startDate, DateTime endDate, int topNumber)
		{
			List<WdsActionRatingListModel> model = new List<WdsActionRatingListModel>();
			try
			{
				var rating = _service.get_rating2024(startDate, endDate, topNumber);
				if (rating.Count > 0)
				{
					foreach (var item in rating)
					{
						if (item.rating_sum > 0)
						{
							WdsActionRatingListModel ratingItem = new WdsActionRatingListModel();
							ratingItem.CustomerContrCode = (string)item.kontr_kod;
							ratingItem.RatingSum = item.rating_sum;

							model.Add(ratingItem);
						}
					}
				}

			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return model;
		}
		public List<Organization1c> GetOrganizationList()
		{
			List<Organization1c> result = new List<Organization1c>();


			// Получаем список контрагентов методом "GetList"

			try
			{
				var res = _service.get_organization_list();

				if (res != null)
				{
					foreach (var item in res)
					{
						result.Add(new Organization1c { Code = (string)item.kod, Name = (string)item.name});
					}
				}
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return result.OrderBy(x => x.Name).ToList();
		}
	}
}