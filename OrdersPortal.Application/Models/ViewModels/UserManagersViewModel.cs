using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class UserManagersViewModel
	{
		public string SalesManagerName { get; set; }
		public string SalesManagerRole { get; set; }
		public string SalesManagerEmail { get; set; }
		public string SalesManagerPhone { get; set; }
		public string SalesManagerImagePath { get; set; }


		public string RegionManagerName { get; set; }
		public string RegionManagerRole { get; set; }
		public string RegionManagerEmail { get; set; }
		public string RegionManagerPhone { get; set; }
		public string RegionManagerImagePath { get; set; }
	}
}
