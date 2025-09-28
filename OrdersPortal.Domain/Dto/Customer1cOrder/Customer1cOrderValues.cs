using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Dto.Customer1cOrder
{
	public class Customer1cOrderValues
	{
		public List<CustomerOrderContragent> ContragentList { get; set; }
		public List<CustomerOrderIban> IbanList { get; set; }
		public List<CustomerOrderAddress> AddressList { get; set; }
		public List<CustomerOrderItem> ItemList { get; set; }

		[Required]
		[Display(Name = "Контрагент")]
		public string Contragent { get; set; }

		[Required]
		[Display(Name = "Рахунок")]
		public string Iban { get; set; }

		[Required]
		[Display(Name = "Адреса")]
		public string Address { get; set; }

		[Required]
		[Display(Name = "Найменування")]
		public string Item { get; set; }

		[Required]
		[Display(Name = "Сума")]
		public decimal Suma { get; set; }

		[Display(Name = "К-сть конструкцій")]
		public int CounstuctionNumber { get; set; }

		[Required]
		[Display(Name = "Дата оплати")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
		public DateTime? PayDate { get; set; }

		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
		[Display(Name = "Дата відвантаження")]
		public DateTime? DeliveryDate { get; set; }

		[Display(Name = "Примітка")]
		public string Description{ get; set; }
	}
}
