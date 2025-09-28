using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Dto.Customer1cOrder
{
	public class Create1COrder
	{

		[Required]
		[Display(Name = "Номер рахунку")]
		public string OrderNumber { get; set; }

		[Required]
		[Display(Name = "Контрагент")]
		public string OrderContrAgent { get; set; }

		[Required]
		[Display(Name = "IBAN")]
		public string OrderIban { get; set; }

		[Required]
		[Display(Name = "Дата оплати")]
		public string OrderCreateDate { get; set; }

		[Required]
		[Display(Name = "Дата відвантаження")]
		public string OrderDeliveryDate { get; set; }

		[Required]
		[Display(Name = "Адреса")]
		public string Address { get; set; }
		
		public List<Create1COrderItem> OrderItems { get; set; }

		[Required]
		[Display(Name = "Сума рахунку")]
		public string OrderSuma { get; set; }

		
		[Display(Name = "Внутрішній коментар")]
		public string InternalComment { get; set; }

		//public List<CustomerOrderItem> Items { get; set; }
		//public List<CustomerOrderContragent> Contragents { get; set; }
		//public List<CustomerOrderIban> Ibans { get; set; }
		//public List<CustomerOrderAddress> Addresses { get; set; }
	}
}
