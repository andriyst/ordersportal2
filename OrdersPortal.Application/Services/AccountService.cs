using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;
using OrdersPortal.Infrastructure.EntityFramework;


namespace OrdersPortal.Application.Services
{
	public class AccountService : IAccountService
	{

		private readonly IAccountRepository _accountRepository;
		private readonly IRegionRepository _regionRepository;
		private readonly ICustomersServices _customersServices;
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IDbContextProvider _dbContextProvider;
		private UserManager<OrderPortalUser> _userManager { get; set; }
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountService(IAccountRepository accountRepository, IRegionRepository regionRepository, ICustomersServices customersServices, IOrganizationRepository organizationRepository)
		{
			_accountRepository = accountRepository;
			_regionRepository = regionRepository;
			_customersServices = customersServices;
			_organizationRepository = organizationRepository;
			_dbContextProvider = DependencyResolver.Current.GetService<IDbContextProvider>();
			_userManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(_dbContextProvider.DbContext));
			_roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContextProvider.DbContext));
		}

		public OrderPortalUser GetById(string accountId)
		{
			if (!String.IsNullOrEmpty(accountId))
			{
				return _accountRepository.GetByIdIncludes(accountId);
			}

			return null;
		}

		public RegisterUserViewModel PrepareRegisterViewModel()
		{
			RegisterUserViewModel model = new RegisterUserViewModel
			{
				Enable = true,
				AutoConfirmOrder = false,
				ExpireDate = DateTime.Today.AddYears(1)
			};

			List<Region> regions = _regionRepository.GetAll().ToList();
			model.RegionList = regions;

			List<Organization> organizations = _organizationRepository.GetAll().ToList();
			//organizations.Add(new Organization { OrganizationId = 0, OrganizationName = "Оберіть організацію" });
			organizations.Insert(0, new Organization { OrganizationName = "Оберіть організацію", OrganizationId = 0 });
			model.OrganizationList = organizations;



			//model.ManagerList = _accountRepository.GetAll().Where(x => x.Manager != null).ToList();
			//model.RegionManagerList = _accountRepository.GetAll().Where(x => x.RegionManager != null).ToList();


			var allRoles = _roleManager.Roles.ToList();

			List<UserRolesViewModel> displayUserRoles = new List<UserRolesViewModel>();

			//model.Managers1cList = _customersServices.GetManagerList();
			//model.RegionManagers1cList = _customersServices.GetRegionManagerList();


			foreach (var a in allRoles)
			{
				if (a.Name == "customer")
				{
					displayUserRoles.Add(new UserRolesViewModel { UserRole = "customer", DisplayUserRole = "Дилер" });
				}
				if (a.Name == "manager")
				{
					displayUserRoles.Add(new UserRolesViewModel { UserRole = "manager", DisplayUserRole = "Менеджер" });
				}
				if (a.Name == "regionmanager")
				{
					displayUserRoles.Add(new UserRolesViewModel { UserRole = "regionmanager", DisplayUserRole = "Регіональний Менеджер" });
				}
				if (a.Name == "operator")
				{
					displayUserRoles.Add(new UserRolesViewModel { UserRole = "operator", DisplayUserRole = "Оператор" });
				}
				if (a.Name == "director")
				{
					displayUserRoles.Add(new UserRolesViewModel { UserRole = "director", DisplayUserRole = "Керівник" });
				}
				if (a.Name == "admin")
				{
					displayUserRoles.Add(new UserRolesViewModel { UserRole = "admin", DisplayUserRole = "Адміністратор" });
				}

			}
			model.RolesList = displayUserRoles;

			model.ContrCodesList = _customersServices.GetCustomerCodesList();

			return model;

		}

		public void AddUser(RegisterUserViewModel model)
		{
			OrderPortalUser dbUser = new OrderPortalUser
			{
				FullName = model.FullUserName,
				Tel = model.Tel,
				Email = model.EMail,
				Enable = model.Enable,
				UserName = model.UserName,
				ExpireDate = model.ExpireDate,
				RegionId = model.RegionId,

				OrderPortalUserOrganizations = model.OrganizationSelectedList.Select(orgId => new OrderPortalUserOrganization{OrganizationId = orgId, JoinedAt = DateTime.Now}).ToList()


			};
			string role = model.Role;

			if (role == "customer")
			{
				
				dbUser.Customer = new Customer
				{
					CustomerContrCode = model.CustomerCode,
					ManagerId = model.ManagerId,
					RegionManagerId = model.RegionManagerId,
					AutoConfirmOrder = model.AutoConfirmOrder,
					PermitFinanceInfo = model.PermitFinanceInfo
				};

			}
			else if (role == "manager")
			{
				dbUser.Manager = new Manager
				{
					Guid = model.ManagerGuid
				};

				UploadPhoto(model.Photo, dbUser.Id);
			}
			else if (role == "regionmanager")
			{

				dbUser.RegionManager = new RegionManager
				{
					Guid = model.RegionManagerGuid
				};

				UploadPhoto(model.Photo, dbUser.Id);
			}
			else if (role == "director")
			{

				dbUser.Director = new Director();

			}
			else if (role == "operator")
			{
				dbUser.Operator = new Operator();
			}
			else
			{
				dbUser.Admin = new Admin();
			}

			var result = _userManager.Create(dbUser, model.Password);

			if (result.Succeeded)
			{

				_userManager.AddToRole(dbUser.Id, model.Role);
			}

		}

		public List<UsersListViewModel> GetUserListByRole(string roleName)
		{
			var roleId = _roleManager.FindByName(roleName).Id;
			//.Users.First();
			var allUsers = _accountRepository.GetAll().Include(x => x.OrderPortalUserOrganizations.Select(o => o.Organization)).Where(u => u.Roles.Select(r => r.RoleId).Contains(roleId)).ToList();

			DateTime now = DateTime.Now.Date;
			DateTime expireDate = DateTime.Now.AddYears(1).Date;

			return allUsers.Select(x => new UsersListViewModel
			{
				UserName = x.UserName,
				FullName = x.FullName,
				EMail = x.Email,
				Tel = x.Tel,
				Region = x.Region != null ? x.Region.RegionName : "Регіон не визначено",
				Enable = x.Enable,
				RegisterDate = x.RegisterDate ?? now,
				ExpireDate = x.ExpireDate ?? expireDate,
				Manager = x.Customer != null ? allUsers.Where(xx => xx.Id == x.Customer.ManagerId).Select(y => y.FullName).FirstOrDefault() : "Менеджер не визначено",
				RegionManager = x.Customer != null ? allUsers.Where(xx => xx.Id == x.Customer.RegionManagerId).Select(y => y.FullName).FirstOrDefault() : "Рег. менеджер не визначено",
				OrganizationsNameList = string.Join("<br />", x.OrderPortalUserOrganizations.Select(y => y.Organization.OrganizationName))

			}).ToList();
		}

		public ChangeUserPasswordViewModel GetUserPassModelByName(string userName)
		{
			var editUser = _accountRepository.GetByUserName(userName);
			return new ChangeUserPasswordViewModel
			{
				UserName = editUser.UserName,
				UserId = editUser.Id
			};
		}

		public void ChangeUserPassword(ChangeUserPasswordViewModel model)
		{

			UserStore<OrderPortalUser> store = new UserStore<OrderPortalUser>(_dbContextProvider.DbContext);

			string hashedNewPassword = _userManager.PasswordHasher.HashPassword(model.NewPassword);


			var cUser = _userManager.FindById(model.UserId);

			store.SetPasswordHashAsync(cUser, hashedNewPassword);
			store.UpdateAsync(cUser);

		}

		public EditUserViewModel PrepareEditUserViewModel(string userName)
		{

			OrderPortalUser user = _accountRepository.GetByUserName(userName);

			var roleName = _userManager.GetRoles(user.Id).FirstOrDefault();
			//string roleName =  _roleManager.Roles.FirstOrDefault(x => x.Id == role.RoleId)?.Name;

			EditUserViewModel model = new EditUserViewModel
			{
				Id = user.Id,
				UserName = user.UserName,
				FullUserName = user.FullName,
				EMail = user.Email,
				Tel = user.Tel,
				RegionId = user.RegionId ?? 0,
				ExpireDate = user.ExpireDate ?? DateTime.Now.AddYears(1).Date,
				Enable = user.Enable,
				Role = roleName
			};

			List<Region> regions = _regionRepository.GetAll().ToList();
			model.RegionList = regions;


			List<Organization> organizations = _organizationRepository.GetAll().ToList();
		
			organizations.Insert(0, new Organization { OrganizationName = "Оберіть організацію", OrganizationId = 0 });
			model.OrganizationList = organizations;

			model.ManagerList = _accountRepository.GetAll().Where(x => x.Manager != null).ToList();
			model.RegionManagerList = _accountRepository.GetAll().Where(x => x.RegionManager != null).ToList();


			//model.Managers1cList = _customersServices.GetManagerList();
			//model.RegionManagers1cList = _customersServices.GetRegionManagerList();


			if (roleName == "customer")
			{
				model.AutoConfirmOrder = user.Customer.AutoConfirmOrder ?? false;
				model.ManagerId = user.Customer.ManagerId;
				model.RegionManagerId = user.Customer.RegionManagerId;
				model.CustomerCode = user.Customer.CustomerContrCode;
				model.AutoConfirmOrder = user.Customer.AutoConfirmOrder ?? false;
				model.PermitFinanceInfo = user.Customer.PermitFinanceInfo ?? false;

			}
			else if (roleName == "manager")
			{
				model.ManagerGuid = user.Manager.Guid;
				model.ManagerPhotoPath = GetPhoto(user.Id);

			}
			else if (roleName == "regionmanager")
			{
				model.RegionManagerGuid = user.RegionManager.Guid;
				model.ManagerPhotoPath = GetPhoto(user.Id);
			}
			model.ContrCodesList = _customersServices.GetCustomerCodesList();
			return model;
		}

		public void EditUser(EditUserViewModel model)
		{

			OrderPortalUser dbOrderPortalUser = _accountRepository.GetByUserName(model.UserName);

			dbOrderPortalUser.UserName = model.UserName;
			dbOrderPortalUser.FullName = model.FullUserName;
			dbOrderPortalUser.Tel = model.Tel;
			dbOrderPortalUser.Email = model.EMail;
			dbOrderPortalUser.Enable = model.Enable;
			dbOrderPortalUser.ExpireDate = model.ExpireDate;
			dbOrderPortalUser.RegionId = model.RegionId;

			if (model.Role == "customer")
			{
				dbOrderPortalUser.Customer.ManagerId = model.ManagerId;
				dbOrderPortalUser.Customer.RegionManagerId = model.RegionManagerId;
				dbOrderPortalUser.Customer.CustomerContrCode = model.CustomerCode;
				dbOrderPortalUser.Customer.AutoConfirmOrder = model.AutoConfirmOrder;
				dbOrderPortalUser.Customer.PermitFinanceInfo = model.PermitFinanceInfo;
			}
			else if (model.Role == "manager")
			{

				dbOrderPortalUser.Manager.Guid = model.ManagerGuid;
				UploadPhoto(model.Photo, model.Id);

			}
			else if (model.Role == "regionmanager")
			{

				dbOrderPortalUser.RegionManager.Guid = model.RegionManagerGuid;
				UploadPhoto(model.Photo, model.Id);

			}

			_accountRepository.SaveChanges();
		}

		public bool UpdateManagers(Update1cManagersDto model)
		{
			return _customersServices.UpdateManagers(model);
		}

		public UserManagersViewModel GetUserManagers(string accountId)
		{
			UserManagersViewModel model = new UserManagersViewModel();
			OrderPortalUser customer = GetById(accountId);
			OrderPortalUser manager = GetById(customer.Customer.ManagerId);
			OrderPortalUser regmanager = GetById(customer.Customer.RegionManagerId);
			if (regmanager != null)
			{
				model.RegionManagerName = regmanager.FullName;
				model.RegionManagerRole = "директор з питань регіонального розвитку";
				model.RegionManagerEmail = regmanager.Email;

				//.Insert(0, "+38").Insert(3, " (").Insert(8, ") ").Insert(13, " ").Insert(16, " ")
				model.RegionManagerPhone = regmanager.Tel.Length == 10 ? regmanager.Tel.Insert(0, "+38").Insert(3, " (").Insert(8, ") ").Insert(13, " ").Insert(16, " ") : regmanager.Tel;
				model.RegionManagerImagePath = GetPhoto(regmanager.Id);
			}

			if (manager != null)
			{

				model.SalesManagerName = manager.FullName;
				model.SalesManagerRole = "менеджер з розвитку";
				model.SalesManagerEmail = manager.Email;
				model.SalesManagerPhone = manager.Tel.Length == 10 ? manager.Tel.Insert(0, "+38").Insert(3, " (").Insert(8, ") ").Insert(13, " ").Insert(16, " ") : manager.Tel;
				model.SalesManagerImagePath = GetPhoto(manager.Id);
			}

			return model;
		}

		public OrganizationCustomerCodesViewModel GetContrAgentCodesByOrg(int orgId)
		{

			OrganizationCustomerCodesViewModel model = new OrganizationCustomerCodesViewModel();
			var organization = _organizationRepository.GetById(orgId);

			var contrAgentCodes = _customersServices.GetCustomerCodesListByOrganization(organization.Organization1cId);
			model.ContrCodesList = contrAgentCodes;
			return model;
		}

		public RegionManagersViewModel GetRegionManagersByOrg(int orgId)
		{

			RegionManagersViewModel model = new RegionManagersViewModel();

			model.RegionManagerList = _accountRepository.GetAll().Where(x => x.RegionManager != null && x.OrderPortalUserOrganizations.Any(o => o.OrganizationId == orgId)).ToList();
			//model.RegionManagerList = _accountRepository.GetAll().Where(x => x.RegionManager != null).ToList();

			return model;
		}

		public ManagersViewModel GetManagersByOrg(int orgId)
		{

			 ManagersViewModel model = new  ManagersViewModel();

			model.ManagerList = _accountRepository.GetAll().Where(x => x. Manager != null && x.OrderPortalUserOrganizations.Any(o => o.OrganizationId == orgId)).ToList();

			return model;
		}


		public RegionManagers1cGuidsViewModel GetRegionManagers1cGuidsByOrg(int orgId)
		{

			RegionManagers1cGuidsViewModel model = new RegionManagers1cGuidsViewModel();
			var organization = _organizationRepository.GetById(orgId);
			model.RegionManagers1cList = _customersServices.GetRegionManagerList(organization.Organization1cId);
			
			return model;
		}

		public Managers1cGuidsViewModel GetManagers1cGuidsByOrg(int orgId)
		{

			Managers1cGuidsViewModel model = new Managers1cGuidsViewModel();
			var organization = _organizationRepository.GetById(orgId);
			model.Managers1cList = _customersServices.GetManagerList(organization.Organization1cId);			

			return model;
		}

		private void UploadPhoto(HttpPostedFileBase photo, string id)
		{
			if (photo != null)
			{
				string filesPath = "/Files/ManagerPhotos/";

				var file = Path.GetFileName(photo.FileName);


				string fullPath = filesPath;

				string fileNameOnly = Path.GetFileName(file);


				string path = Path.Combine(HttpContext.Current.Server.MapPath(fullPath), id + ".jpg");

				string correctFilename = HttpContext.Current.Server.UrlPathEncode(Path.GetFileName(path));
				HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + correctFilename + "\"");

				photo.SaveAs(path);
			}
		}

		private string GetPhoto(string id)
		{
			string photoUrl = "/Files/ManagerPhotos/no-photo.jpg";

			var path = $"/Files/ManagerPhotos/{id}.jpg";
			var fullPath = HttpContext.Current.Server.MapPath(path);

			if (File.Exists(fullPath))
			{
				photoUrl = path;
			}

			return photoUrl;
		}
	}
}
