using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class RegionAddViewModel
	{
	
		[Required]
		[Display(Name = "Назва регіону")]
		public string RegionName { get; set; }

	}

}