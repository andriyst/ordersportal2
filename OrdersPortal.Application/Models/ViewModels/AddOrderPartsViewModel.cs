using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class AddOrderPartsViewModel
	{

		[Required]
		[Display(Name = "Прізвище контрагента")]
		public string CustomerName { get; set; }


		[Display(Name = "Прізвище менеджера")]
		public string ManagerName { get; set; }


		//Номер замовлення до якого буде дозамовлення
		[Display(Name = "Номер замовлення")]
		public string OrderNumber { get; set; }


		//[Required]
		//[Display(Name = "Номер замовл.")]
		//[RegularExpression("([а-яА-ЯіІїЇєЄa-zA-Z0-9 .&'-_№#]{0,11}$)", ErrorMessage = "Некоректні символи або більше 11 символів")]
		//public string OrderPartsNumber { get; set; }

		[Display(Name = "Коментар контрагента")]
		public string OrderPartsDescription { get; set; }

		
		public int OrderPartsItemsId { get; set; }
		[Display(Name = "Елемент на дозамовлення")]
		public virtual List<SelectListItem> OrderPartsItems { get; set; }


		
		public int OrderPartsReasonId { get; set; }
		[Display(Name = "Причина дозамовлення")]
		public virtual List<SelectListItem> OrderPartsReason { get; set; }


		//Номер замовлення до якого буде дозамовлення
		[Display(Name = "Дата відправки на виробництво")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
		public DateTime? OrderPartsDepartureDate { get; set; }



	}
}
