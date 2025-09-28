using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;

namespace OrdersPortal.Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly IStatusRepository _statusRepository;
		private readonly IDb1SOrderNumbersRepository _db1SOrderNumbersRepository;
		private readonly IAccountService _accountService;
		private readonly ICustomersServices _customersServices;
		private readonly ApplicationContext _applicationContext;

		private const int InitOrderStatusId = 1;

		public OrderService(IOrderRepository orderRepository, IAccountRepository accountRepository,
			IStatusRepository statusRepository, IDb1SOrderNumbersRepository db1SOrderNumbersRepository,
			IAccountService accountService, ICustomersServices customersServices, ApplicationContext applicationContext)
		{
			_orderRepository = orderRepository;
			_accountRepository = accountRepository;
			_statusRepository = statusRepository;
			_db1SOrderNumbersRepository = db1SOrderNumbersRepository;
			_accountService = accountService;
			_customersServices = customersServices;
			_applicationContext = applicationContext;
		}

		public bool CheckContrAgentOrder(string order1CNumber)
		{
			try
			{
				OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);

				if (currentUser?.Customer != null)
				{

					DateTime date = DateTime.Today.AddYears(-20);

					return _customersServices.CheckOrderInPeriodService(currentUser.Customer?.CustomerContrCode, order1CNumber, date);

				}
				return false;
			}
			catch (Exception ex)
			{
				ex = ex;
				return false;
			}
		}

		public void UploadOrder(UploadOrderViewModel order)
		{

			string filesPath = "/Files/Orders/";
			var realFile = order.File;

			var file = Path.GetFileName(realFile.FileName);

			string customerId = _applicationContext.AccountId;

			string fullPath = filesPath + customerId;


			if (!Directory.Exists(HttpContext.Current.Server.MapPath(fullPath)))
			{
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath(fullPath));
			}

			string fileNameOnly = Path.GetFileNameWithoutExtension(file);
			string extension = Path.GetExtension(file);

			string path = Path.Combine(HttpContext.Current.Server.MapPath(fullPath), fileNameOnly + extension);

			int count = 1;

			while (File.Exists(path))
			{
				string tempFileName = $"{fileNameOnly}({count++})";
				path = Path.Combine(HttpContext.Current.Server.MapPath(fullPath), tempFileName + extension);
			}

			string correctFilename = HttpContext.Current.Server.UrlPathEncode(Path.GetFileName(path));
			HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + correctFilename + "\"");

			order.File.SaveAs(path);


			var managerId = _accountRepository.GetByIdIncludes(customerId).Customer.ManagerId;

			Order tmpOrder = new Order
			{
				File = fullPath + "/" + correctFilename,
				OrderDateCreate = DateTime.Now,
				OrderNumberContructions = order.OrderNumberContructions,
				OrderNumber = order.OrderNumber,
				CustomerId = customerId,
				ManagerId = managerId,
				StatusId = InitOrderStatusId,
				LastMessageTime = DateTime.Now

			};
			List<OrdersMessage> tmpOrderMess = new List<OrdersMessage>
			{
				new OrdersMessage
				{
					Message = order.CommentMessage,
					MessageWriterId = customerId,
					OrderId = tmpOrder.OrderId
				}
			};

			tmpOrder.OrdersMessage = tmpOrderMess;

			_orderRepository.AddPermanent(tmpOrder);

			//var _service = new EmailService();
			//_service.UploadOrderEmailNotify(tmpOrder);

		}

		public void RemoveOrder(int orderId)
		{
			Order order = _orderRepository.GetById(orderId);
			_orderRepository.Remove(order);
		}
		public void ChangeOrderStatusById(int orderId, string statusName)  //todo: refactor not only on Upload status
		{
			var order = _orderRepository.GetById(orderId);

			var status = _statusRepository.GetStatusByName(statusName);

			if (order != null && order.StatusId == 1)
			{
				order.StatusId = status.StatusId;

			}
			_orderRepository.SaveChanges();
		}

		public void Confirm1SOrder(int db1SOrderNumberId)
		{
			var db1COrder = _db1SOrderNumbersRepository.GetByIdIncludes(db1SOrderNumberId);

			if (db1COrder != null)
			{
				var customer = _accountRepository.GetByIdIncludes(db1COrder.Order.CustomerId).Customer;

				string contrCode = CustomerHelper.GetContrAgentFullCode(customer.CustomerContrCode.ToString());

				var org1cCode = customer.OrderPortalUser.OrderPortalUserOrganizations.FirstOrDefault().Organization.Organization1cId;

				if (_customersServices.Set1CDeliveryStatus(contrCode, db1COrder.Db1SOrderNumber, org1cCode))
				{
					db1COrder.Db1SOrderConfirm = true;
					db1COrder.Db1SOrderStatus = "Очікує оплати";
					if (db1COrder.Order.Db1SOrderNumbers.All(c => c.Db1SOrderConfirm))
						db1COrder.Order.StatusId = 22;

					_db1SOrderNumbersRepository.SaveChanges();
				}
			}
		}
		public List<Db1SOrderNumbers> Get1COrdersByOrderId(int orderId)
		{
			return _db1SOrderNumbersRepository.GetListByOrderId(orderId);
		}

		public List<string> Get1COrderNumberByOrderId(int orderId)
		{
			return _db1SOrderNumbersRepository.GetNumbersListByOrderId(orderId);
		}

		public byte[] Get1COrderNumberFileByOrderId(string db1cOrderNumber)
		{
			var result = _db1SOrderNumbersRepository.GetDb1SOrderNumberByNumber(db1cOrderNumber);
			return result.OrderImage;
		}
	}
}
