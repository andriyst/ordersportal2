using System;

namespace OrdersPortal.Domain.Models
{
	public class FinanceOrder
	{
		public string Number { get; set; }
		public DateTime? StartDate { get; set; }
		public string ReadyDate { get; set; }
		public string DelayDate { get; set; }
		public string ShipmentDate { get; set; }
		public string DeliveryDate { get; set; }
		public int QuantityConstructions { get; set; }
		public decimal TotalValue { get; set; }
		public decimal AdvanceValue { get; set; }
		public decimal BalanceValue { get; set; }
		public string Status { get; set; }
		public string Agreement { get; set; }

	}
}
