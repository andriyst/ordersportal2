using System.Collections.Generic;
using System.Collections.ObjectModel;
using OrdersPortal.Domain.Dto.Customer1cOrder;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class Create1COrderViewModel : Create1COrder
	{
		//public string OrderNumber { get; set; }
		//public string OrderContrAgent { get; set; }
		//public string OrderIban { get; set; }
		//public string OrderCreateDate { get; set; }
		//public string OrderDeliveryDate { get; set; }
		//public string Address { get; set; }
		//public List<Create1COrderItem> OrderItems { get; set; }
		//public string OrderSuma { get; set; }
		//public string InternalComment { get; set; }

		public List<CustomerOrderItem> Items { get; set; }
		public List<CustomerOrderContragent> Contragents { get; set; }
		public List<CustomerOrderIban> Ibans { get; set; }
		public List<CustomerOrderAddress> Addresses { get; set; }
	}
}
