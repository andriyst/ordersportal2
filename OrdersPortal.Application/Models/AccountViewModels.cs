using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models
{

    public class ChangeUserPasswordViewModel
    {

        public string UserId { get; set;}
        public string UserName { get; set;}

        [Required]
        [StringLength(100, ErrorMessage = "{0} повинен містити щонайменше {2} символів", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        [Compare("NewPassword", ErrorMessage = "Новий пароль та його підтвердження не співпадають")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "Відправити повідомлення користувачу")]
        public Boolean NotifyUser { get; set; }

    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Поточний пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} повинен містити щонайменше {2} символів", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження паролю")]
        [Compare("NewPassword", ErrorMessage = "Новий пароль та його підтвердження не співпадають")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Логін")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запамятати мене?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name = "Логін")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} має містити щонайменше {2} символів.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження пароля")]
        [Compare("Password", ErrorMessage = "Пароль та його підтвердження не співпадають.")]
        public string ConfirmPassword { get; set; }

		[Required]
		[Display(Name = "Прізвище та ім'я")]
		public string FullUserName { get; set; }

		[Required]
		[Display(Name = "E-mail")]
		public string EMail { get; set; }

		[Required]
		[Display(Name = "Телефон")]
		public string Tel { get; set; }

		
		[Display(Name = "Регіон")]
		public int RegionId { get; set; }
		
		[Required]
		[Display(Name = "Тип користувача")]
		public string Role { get; set; }

		[Required]
		[Display(Name = "Активний")]
		public bool Enable { get; set; }

		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
		[Display(Name = "Активний до")]
		public DateTime ExpireDate { get; set; }

		
		[Display(Name = "Код Контрагента")]
		public int CustomerCode { get; set; }

		
		[Display(Name = "Менеджер")]
		public string ManagerId { get; set; }

		
		[Display(Name = "Рег. Менеджер")]
		public string RegionManagerId { get; set; }

		[Required]
		[Display(Name = "Фінансові операції")]
		public bool PermitFinanceInfo { get; set; }

		[Required]
        [Display(Name = "Авто Підтвердження")]
        public bool AutoConfirmOrder { get; set; }

        [Display(Name = "Менеджер Guid")]
        public string ManagerGuid { get; set; }

        [Display(Name = "Рег. Менеджер Guid")]
        public string RegionManagerGuid { get; set; }
        
        [Display(Name = "Фото")]
        public HttpPostedFileBase Photo { get; set; }

		[Display(Name = "Обрати Організацію")]		
		public List<int> OrganizationSelectedList { get; set; }
		public virtual List<Organization> OrganizationList { get; set; }

		public virtual List<Region> RegionList { get; set; }
	
		public virtual List<OrderPortalUser> ManagerList { get; set; }
		public virtual List<OrderPortalUser> RegionManagerList { get; set; }
		public List<UserRolesViewModel> RolesList { get; set; }

		public List<Manager1c> Managers1cList { get; set; } 
		public List<RegionManager1c> RegionManagers1cList { get; set; }
		public List<CustomerCodesListDto> ContrCodesList { get; set; }


	}



	public class EditUserViewModel
	{

		public string Id  { get; set; }

		[Required]
		[Display(Name = "Логін")]
		public string UserName { get; set; }

		
		[Required]
		[Display(Name = "Прізвище та ім'я")]
		public string FullUserName { get; set; }

		[Required]
		[Display(Name = "E-mail")]
		public string EMail { get; set; }

		[Required]
		[Display(Name = "Телефон")]
		public string Tel { get; set; }


		[Display(Name = "Регіон")]
		public int RegionId { get; set; }

		[Required]
		[Display(Name = "Тип користувача")]
		public string Role { get; set; }

		[Required]
		[Display(Name = "Активний")]
		public bool Enable { get; set; }

		[Required]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
		[Display(Name = "Активний до")]
		public DateTime ExpireDate { get; set; }


		[Display(Name = "Код Контрагента")]
		public int CustomerCode { get; set; }


		[Display(Name = "Менеджер")]
		public string ManagerId { get; set; }


		[Display(Name = "Менеджер Guid")]
		public string ManagerGuid { get; set; }

		[Display(Name = "Рег. Менеджер Guid")]
		public string RegionManagerGuid { get; set; }

		[Display(Name = "Обрати Організацію")]
		public List<int> OrganizationSelectedList { get; set; }
		



		[Display(Name = "Рег. Менеджер")]
		public string RegionManagerId { get; set; }

		[Required]
		[Display(Name = "Фінансові операції")]
		public bool PermitFinanceInfo { get; set; }
		[Required]
        [Display(Name = "Авто Підтвердження")]
        public bool AutoConfirmOrder { get; set; }

        [Display(Name = "Фото")]
        public HttpPostedFileBase Photo { get; set; }
		public string ManagerPhotoPath { get; set; }

		public virtual List<Region> RegionList { get; set; }
		public virtual List<Organization> OrganizationList { get; set; }
		public virtual List<OrderPortalUser> ManagerList { get; set; }
        public virtual List<OrderPortalUser> RegionManagerList { get; set; }
        public List<UserRolesViewModel> RolesList { get; set; }

        public List<Manager1c> Managers1cList { get; set; }
        public List<RegionManager1c> RegionManagers1cList { get; set; }
        public List<CustomerCodesListDto> ContrCodesList { get; set; }

	}
	public class UserRolesViewModel
	{
		public string UserRole { get; set; }

		public string DisplayUserRole { get; set; }
	}
}
