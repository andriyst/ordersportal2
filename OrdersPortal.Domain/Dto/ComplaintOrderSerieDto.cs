using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Dto
{
	public class ComplaintOrderSerieDto
	{
		public int Id { get; set; }
		public string SerieGuid { get; set; }
		public string SerieName { get; set; }
		public string SerieCategory { get; set; }
		public bool Checked { get; set; }
		public int ComplaintId { get; set; }
		public int OrderId { get; set; }
	}
}
