using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class ManagerCommentViewModel
	{
		[Display(Name = "Id")]
		public int OrderId { get; set; }

		[Display(Name = "Коментар менеджера")]
		public string ManagerComment { get; set; }
	}
}