using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Entities
{
	public class OrderPartsItem
	{
		[Key]
		public int OrderPartsItemId { get; set; }
		public string OrderPartsItemName { get; set; }
		public string OrderPartsItemGuid { get; set; }

	}

}
