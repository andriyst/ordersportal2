using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Models.Analytics
{
	public class AnalyticsOrder
	{
		
		public decimal TotalValue { get; set; }
		public decimal AvarageBill { get; set; }
		public string Category { get; set; }
		public int  ActualyProductionDays { get; set; }

		public string ProfileSystem { get; set; }

		public string Number { get; set; }
		public DateTime? StartDate { get; set; }
		public string ReadyDate { get; set; }
		public string DelayDate { get; set; }
		public string ShipmentDate { get; set; }
		public string DeliveryDate { get; set; }
		public int QuantityConstructions { get; set; }
		
		public decimal AdvanceValue { get; set; }
		public decimal BalanceValue { get; set; }
		public string Status { get; set; }
		public string Agreement { get; set; }
	}
}
