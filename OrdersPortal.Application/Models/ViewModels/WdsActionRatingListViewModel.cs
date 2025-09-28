using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class WdsActionRatingListViewModel
	{
		public int CustomerContrCode { get; set; }
		public double RatingValue { get; set; }
		public double RatingSum { get; set; }
		public string CustomerName { get; set; }
	}
}
