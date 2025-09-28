using System;
using System.Collections.Generic;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrdersPaymentsViewModel
	{
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string ContrAgentCode { get; set; }
		public List<OrderPayment> Payments { get; set; }
		public decimal AvailableAdvance { get; set; }
	}
}
