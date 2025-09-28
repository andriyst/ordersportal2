using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class UsersListViewModel
	{
		[Required]
		[Display(Name = "Логін")]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "Повне ім'я")]
		public string FullName { get; set; }

		[Required]
		[Display(Name = "E-mail")]
		public string EMail { get; set; }


		[Display(Name = "Телефон")]
		public string Tel { get; set; }


		[Display(Name = "Регіон")]
		public string Region { get; set; }
		//---------------------------------------------------

		[Display(Name = "Менеджер")]
		public string Manager { get; set; }

		[Display(Name = "Код Контрагента")]
		public int CustomerContrCode { get; set; }
		//----------------------------------------------------

		[Display(Name = "Рег. Менеджер")]
		public string RegionManager { get; set; }
		//----------------------------------------------------

		[Required]
		[Display(Name = "Активовано")]
		public bool Enable { get; set; }


		[Display(Name = "Дата Реєстрації")]
		public DateTime RegisterDate { get; set; }


		[Display(Name = "Активований До")]
		public DateTime ExpireDate { get; set; }

		[Display(Name = "Організація")]
		public string OrganizationsNameList { get; set; }

	}



}