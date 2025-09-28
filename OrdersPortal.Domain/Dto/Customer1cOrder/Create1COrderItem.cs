using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrdersPortal.Domain.Dto.Customer1cOrder;

namespace OrdersPortal.Domain.Dto.Customer1cOrder
{
	public class Create1COrderItem
	{
		public CustomerOrderItem OrderItem { get; set; }
		public int? Width { get; set; }
		public int? Height { get; set; }
		public string Price { get; set; }
		public string Count { get; set; }
	}
}
