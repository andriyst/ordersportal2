using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models
{
	public class OrderPayment
	{
		public DateTime? OrderDate { get; set; }
		public string OrderNumber { get; set; }
		public decimal Price { get; set; }
		public decimal  CurrentAdvance { get; set; }
		public decimal Payment { get; set; }
		public string Status { get; set; }
		public string Agreement { get; set; }


	}
}
