using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Models
{
	public class FinanceTableDataModel
	{
		public string Order { get; set; }
		public string Sort { get; set; }
		public string Search { get; set; }
		public int Offset { get; set; }
		public int Limit { get; set; }

		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

		public string ContrAgentCode { get; set; }
	}
}
