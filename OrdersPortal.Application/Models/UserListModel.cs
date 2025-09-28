using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models
{
	public class UserListModel
	{
		public IList<OrderPortalUser> UserList { get; set; }
		public string UserListName { get; set; }
	}
}
