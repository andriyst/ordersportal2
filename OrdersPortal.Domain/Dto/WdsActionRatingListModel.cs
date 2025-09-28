using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Dto
{
	public class WdsActionRatingListModel
	{
		public string CustomerContrCode { get; set; }		
		public double RatingValue { get; set; }
		public double RatingSum { get; set; }
		public string CustomerName { get; set; }

	}
}
