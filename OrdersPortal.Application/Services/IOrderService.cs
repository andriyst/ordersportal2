using System.Collections.Generic;
using System.Web.Mvc;

using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Services
{
	public interface IOrderService
	{
		bool CheckContrAgentOrder(string order1CNumber);
		void UploadOrder(UploadOrderViewModel order);
		void RemoveOrder(int orderId);
		void ChangeOrderStatusById(int orderId, string statusName);
		void Confirm1SOrder(int db1SOrderNumberId);
		List<Db1SOrderNumbers> Get1COrdersByOrderId(int orderId);
		List<string> Get1COrderNumberByOrderId(int orderId);
		byte[] Get1COrderNumberFileByOrderId(string db1cOrderNumber);
	}

}
