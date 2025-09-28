using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Dto.Customer1cOrder
{
	public class CustomerBillListDto
	{
		public string Guid { get; set; }
		public string Number { get; set; }
		public decimal Suma { get; set; }
		public DateTime Date { get; set; }
	}
}
