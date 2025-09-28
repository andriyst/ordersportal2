using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Services;

namespace OrdersPortal.Application.Services
{
	public class OrderPartsService : IOrderPartsService
	{
		private readonly IOrderPartsRepository _orderPartsRepository;
		private readonly IOrderPartsItemRepository _orderPartsItemRepository;
		private readonly IOrderPartsReasonRepository _orderPartsReasonRepository;
		private readonly IDb1SOrderPartsNumbersRepository _db1SOrderPartsNumbersRepository;
		private readonly ICustomersServices _customersServices;
		private readonly IAccountService _accountService;
		private readonly IAccountRepository _accountRepository;

		private readonly ApplicationContext _applicationContext;
		private readonly UserManager<OrderPortalUser> _userManager;

		private const int InitOrderStatusId = 1;


		public OrderPartsService(ApplicationContext applicationContext, IOrderPartsRepository orderPartsRepository, IAccountService accountService, IAccountRepository accountRepository,
			IOrderPartsReasonRepository orderPartsReasonRepository, IOrderPartsItemRepository orderPartsItemRepository, IDb1SOrderPartsNumbersRepository db1SOrderPartsNumbersRepository,
			ICustomersServices customersServices)
		{
			
			_orderPartsRepository = orderPartsRepository;
			_accountRepository = accountRepository;
			_orderPartsReasonRepository = orderPartsReasonRepository;
			_orderPartsItemRepository = orderPartsItemRepository;
			_db1SOrderPartsNumbersRepository = db1SOrderPartsNumbersRepository;
			_accountService = accountService;
			_customersServices = customersServices;

			_applicationContext = applicationContext;
			var dbContextProvider = DependencyResolver.Current.GetService<IDbContextProvider>();
			_userManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(dbContextProvider.DbContext));
		}

		public AddOrderPartsViewModel PrepareAddViewModel() {

			AddOrderPartsViewModel viewModel = new AddOrderPartsViewModel();

			
			viewModel.OrderPartsReason =  _orderPartsReasonRepository.GetAll().Select(x => new SelectListItem {Text=x.OrderPartsReasonName, Value=x.OrderPartsReasonId.ToString() } ).ToList();


			viewModel.OrderPartsItems = _orderPartsItemRepository.GetAll().Select(x => new SelectListItem { Text = x.OrderPartsItemName, Value = x.OrderPartsItemId.ToString() }).ToList(); 

		

			OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);
			viewModel.CustomerName = currentUser.FullName;

			return viewModel;

		}

		public void AddOrderParts(AddOrderPartsViewModel viewModel) {

			string customerId = _applicationContext.AccountId;
			var managerId = _accountRepository.GetByIdIncludes(customerId).Customer.ManagerId;

			OrderParts orderParts = new OrderParts
			{
				ManagerId = managerId,
				CustomerId = customerId,
				OrderNumber = viewModel.OrderNumber,
				OrderPartsItemId = viewModel.OrderPartsItemsId,
				OrderPartsDate = DateTime.Now,
				OrderPartsDescription = viewModel.OrderPartsDescription,
				OrderPartsDepartureDate = viewModel.OrderPartsDepartureDate ?? null,
				OrderPartsReasonId = viewModel.OrderPartsReasonId,
				StatusId = InitOrderStatusId


			};

			_orderPartsRepository.AddPermanent(orderParts);
		}

		public string Get1COrderPartsNumberByOrderId(int orderPartsId)
		{
			return _db1SOrderPartsNumbersRepository.GetNumberByOrderPartsId(orderPartsId);
		}
		public Db1SOrderPartsNumbers Get1COrderPartsByOrderPartsId(int orderPartsId)
		{
			return _db1SOrderPartsNumbersRepository.GetByOrderPartsId(orderPartsId);
		}
		public byte[] Get1COrderPartsNumberFileByOrderPartsId(string db1cOrderPartsNumber)
		{
			var result = _db1SOrderPartsNumbersRepository.GetDb1SOrderPartsNumberByNumber(db1cOrderPartsNumber);
			return result.OrderImage;
		}

		public void Confirm1SOrderParts(int db1SOrderPartsNumberId)
		{
			var db1COrder = _db1SOrderPartsNumbersRepository.GetByIdIncludes(db1SOrderPartsNumberId);

			if (db1COrder != null)
			{
				var customer = _accountRepository.GetByIdIncludes(db1COrder.OrderParts.CustomerId).Customer;

				string contrCode = CustomerHelper.GetContrAgentFullCode(customer.CustomerContrCode.ToString());



				if (_customersServices.Set1COrderPartsDeliveryStatus(contrCode, db1COrder.OrderPartsId))
				{
					db1COrder.Db1SOrderPartsConfirm = true;
					db1COrder.Db1SOrderPartsStatus = "В роботі";
					if (db1COrder.Db1SOrderPartsConfirm)
						db1COrder.OrderParts.StatusId = 14;

					_db1SOrderPartsNumbersRepository.SaveChanges();
				}
			}
		}
	}
}
