using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Entities
{
	public class OrderPartsReason
	{
		[Key]
		public int OrderPartsReasonId { get; set;}
		public string OrderPartsReasonName { get; set; }
		public string OrderPartsReasonGuid { get; set; }

	}
}
