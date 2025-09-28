using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using NLog;
using System.Data.Entity;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.WebUI.Controllers
{

	[Authorize]
	public class AccountController : Controller
	{

		private readonly IAccountRepository _accountRepository;
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IAccountService _accountService;
		private readonly ApplicationContext _applicationContext;
		private Logger _logger;
		public UserManager<OrderPortalUser> UserManager { get; private set; }

		public AccountController(IAccountRepository accountRepository, IAccountService accountService, IOrganizationRepository organizationRepository,
			ApplicationContext applicationContext)
		{
			_accountRepository = accountRepository;
			_accountService = accountService;
			_organizationRepository = organizationRepository;
			_applicationContext = applicationContext;
			_logger = LogManager.GetCurrentClassLogger();
			var dbContextProvider = DependencyResolver.Current.GetService<IDbContextProvider>();
			UserManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(dbContextProvider.DbContext));

		}



		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (ModelState.IsValid)
			{

				var user = await UserManager.FindAsync(model.UserName, model.Password);
				if (user != null && user.Enable && user.ExpireDate > DateTime.Now)
				{
					await SignInAsync(user, model.RememberMe);
					_logger.Debug("User: {0} login Successful", model.UserName);
					_logger.Debug("User: {0}, Password: {1}", model.UserName, model.Password);
					return RedirectToLocal(returnUrl);
				}
				else
				{
					_logger.Error("User: {0} login Error", model.UserName);
					ModelState.AddModelError("", "Не вірний логін або пароль.");
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Account/Register
		[Authorize(Roles = "admin")]
		public ActionResult Register()
		{
			var model = _accountService.PrepareRegisterViewModel();

			return View(model);
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[Authorize(Roles = "admin")]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterUserViewModel model)
		{
			if (ModelState.IsValid)
			{
				_accountService.AddUser(model);
				_logger.Debug("User: {0} created Successful", model.UserName);
			}
			else {
				_logger.Debug("User {0} creation failed: {1} ", model.UserName, ModelState.Count);

			}
			
			// If we got this far, something failed, redisplay form
			return RedirectToAction("UsersList", "Account");
		}

		//
		// GET: /Account/Manage
		public ActionResult Manage(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Ваш пароль змінено"
				: message == ManageMessageId.SetPasswordSuccess ? "Ваш пароль встановлено"
				: message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "Сталася помилка"
				: "";
			ViewBag.HasLocalPassword = HasPassword();
			ViewBag.ReturnUrl = Url.Action("Manage");
			return View();
		}

		//
		// POST: /Account/Manage
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Manage(ManageUserViewModel model)
		{
			bool hasPassword = HasPassword();
			ViewBag.HasLocalPassword = hasPassword;
			ViewBag.ReturnUrl = Url.Action("Manage");
			if (hasPassword)
			{
				if (ModelState.IsValid)
				{
					IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
					if (result.Succeeded)
					{
						_logger.Debug("User: {0} change password Successful", User.Identity.GetUserId());
						return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
					}
					else
					{
						_logger.Error("User: {0} change password Error:{1}", User.Identity.GetUserId(), result);
						AddErrors(result);
					}
				}
			}
			else
			{
				// User does not have a password so remove any validation errors caused by a missing OldPassword field
				ModelState state = ModelState["OldPassword"];
				if (state != null)
				{
					state.Errors.Clear();
				}

				if (ModelState.IsValid)
				{
					IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
					if (result.Succeeded)
					{
						return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
					}
					else
					{
						_logger.Error("User: {0} change password Error:{1}", User.Identity.GetUserId(), result);
						AddErrors(result);
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}


		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut();
			return RedirectToAction("Index", "Home");
		}


		[ChildActionOnly]
		public ActionResult RemoveAccountList()
		{
			var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
			ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
			return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && UserManager != null)
			{
				UserManager.Dispose();
				UserManager = null;
			}
			base.Dispose(disposing);
		}

		//[Authorize(Roles = "admin")]
		//public ActionResult UsersList()
		//{
		//	DateTime now = DateTime.Now.Date;
		//	DateTime expireDate = DateTime.Now.AddYears(1).Date;
		//	List<UsersListViewModel> allUserList = _accountRepository.GetAll()
		//		.Include(x => x.OrderPortalUserOrganizations.Select(o => o.Organization))
		//		.ToList()
		//		.Select(x => new UsersListViewModel
		//		{
		//			UserName = x.UserName,
		//			FullName = x.FullName,
		//			EMail = x.Email,
		//			Tel = x.Tel,
		//			Region = x.Region != null ? x.Region.RegionName : "Регіон не визначено",
		//			Enable = x.Enable,
		//			RegisterDate = x.RegisterDate ?? now,
		//			ExpireDate = x.ExpireDate ?? expireDate,
		//			OrganizationsNameList = string.Join("<br />", x.OrderPortalUserOrganizations.Select(y => y.Organization.OrganizationName))

		//		}).ToList();

		//	ViewBag.UsersList = allUserList;


		//	ViewBag.CustomersList = _accountService.GetUserListByRole("customer");

		//	ViewBag.ManagersList = _accountService.GetUserListByRole("manager");

		//	ViewBag.RegionmanagersList = _accountService.GetUserListByRole("regionmanager");

		//	ViewBag.DirectorsList = _accountService.GetUserListByRole("director");

		//	ViewBag.AdminsList = _accountService.GetUserListByRole("admin");


		//	return View();
		//}
		[Authorize(Roles = "admin")]
		public ActionResult UsersList(int?[] organizationIds)
		{
			DateTime now = DateTime.Now.Date;
			DateTime expireDate = DateTime.Now.AddYears(1).Date;

			var allOrganizationsId = _organizationRepository.GetAll().Select(x=>x.OrganizationId);

			if (organizationIds == null || !organizationIds.Any()) {
				organizationIds = allOrganizationsId.Select(id => (int?)id).ToArray();
			}

			List<UsersListViewModel> allUserList = _accountRepository.GetAll()
				.Include(x => x.OrderPortalUserOrganizations.Select(o => o.Organization))
				.Where(x => x.OrderPortalUserOrganizations
					.Any(uo => organizationIds.Contains(uo.OrganizationId)))
				.ToList()
				.Select(x => new UsersListViewModel
				{
					UserName = x.UserName,
					FullName = x.FullName,
					EMail = x.Email,
					Tel = x.Tel,
					Region = x.Region != null ? x.Region.RegionName : "Регіон не визначено",
					Enable = x.Enable,
					RegisterDate = x.RegisterDate ?? now,
					ExpireDate = x.ExpireDate ?? expireDate,
					OrganizationsNameList = string.Join("<br />", x.OrderPortalUserOrganizations.Select(y => y.Organization.OrganizationName))

				}).ToList();

			ViewBag.UsersList = allUserList;


			ViewBag.CustomersList = _accountService.GetUserListByRole("customer");

			ViewBag.ManagersList = _accountService.GetUserListByRole("manager");

			ViewBag.RegionmanagersList = _accountService.GetUserListByRole("regionmanager");

			ViewBag.DirectorsList = _accountService.GetUserListByRole("director");

			ViewBag.AdminsList = _accountService.GetUserListByRole("admin");


			return View();
		}

		[Authorize(Roles = "admin")]
		public ActionResult ChangeUserPassword(string username)
		{
			var model = _accountService.GetUserPassModelByName(username);

			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "admin")]
		public ActionResult ChangeUserPassword(ChangeUserPasswordViewModel model)
		{
			//var user = _accountRepository.GetById(model.UserId);

			_accountService.ChangeUserPassword(model);

			string message = "Пароль користувача " + model.UserName + " успішно змінено";
			return RedirectToAction("UsersList", new { message = message });
		}

		[Authorize(Roles = "admin")]
		public ActionResult EditUser(string userName)
		{

			var model = _accountService.PrepareEditUserViewModel(userName);

			return View(model);
		}

		//
		// POST: /Regions/EditRegion
		[HttpPost]
		[Authorize(Roles = "admin")]
		[ValidateAntiForgeryToken]
		public ActionResult EditUser(EditUserViewModel user)
		{
			_accountService.EditUser(user);

			string message = "Користувач " + user.UserName + " успішно змінено";
			return RedirectToAction("UsersList", new { message = message });
		}

		[ChildActionOnly]
		public ActionResult RegisterManager()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null && User.IsInRole("customer"))
			{

				var code = CustomerHelper.GetContrAgentFullCode(user.Customer.CustomerContrCode.ToString());
				return PartialView("_RegisterManager", code);
			}

			return null;
		}

		[Authorize(Roles = "admin")]
		public ActionResult Update1cManagers()
		{

			var organiztionList = _organizationRepository.GetAll().ToList();

			return View(new Update1cManagersDto
			{
				RegionManagers = true,
				Managers = true,
				UpdatedCustomers = new List<Customer>(),
				OrganizationList = organiztionList
			});
		}
		[Authorize(Roles = "admin")]
		[HttpPost]
		public ActionResult Update1cManagers(Update1cManagersDto model)
		{

			if (_accountService.UpdateManagers(model))
			{
				ViewBag.Result = "Оновлення пройшло успішно";

			}
			else
			{
				ViewBag.Result = "Виникла помилка при оновлені";
			}
			model.OrganizationList = _organizationRepository.GetAll().ToList();
			return View(model);
		}

		public ActionResult ShowFinanceMenu()
		{
			if (_applicationContext.PermitFinanceInfo)
			{
				return PartialView("_FinanceMenu");
			}

			return null;
		}

		public ActionResult ShowUserManagers()
		{
			if (User.IsInRole("customer"))
			{
				UserManagersViewModel viewModel = _accountService.GetUserManagers(User.Identity.GetUserId());
				return PartialView("_UserManagers", viewModel);
			}

			return null;
		}

		public ActionResult LoadCustomerCodesByOrg(int orgId)
		{
			if (orgId != null && orgId > 0)
			{
				OrganizationCustomerCodesViewModel viewModel = _accountService.GetContrAgentCodesByOrg(orgId);
				return PartialView("_CustomerCodesPartialForm", viewModel);
			}

			return null;
		}

		public ActionResult LoadManagersByOrg(int orgId)
		{
			if (orgId != null && orgId > 0)
			{
				ManagersViewModel viewModel = _accountService.GetManagersByOrg(orgId);
				return PartialView("_ManagersPartialForm", viewModel);
			}

			return null;
		}

		public ActionResult LoadRegionManagersByOrg(int orgId)
		{
			if (orgId != null && orgId > 0)
			{
				RegionManagersViewModel viewModel = _accountService.GetRegionManagersByOrg(orgId);
				return PartialView("_RegionManagersPartialForm", viewModel);
			}

			return null;
		}


		public ActionResult LoadManagers1cGuidsByOrg(int orgId)
		{
			if (orgId != null && orgId > 0)
			{
				Managers1cGuidsViewModel viewModel = _accountService.GetManagers1cGuidsByOrg(orgId);
				return PartialView("_Managers1cGuidsPartialForm", viewModel);
			}

			return null;
		}

		public ActionResult LoadRegionManagers1cGuidsByOrg(int orgId)
		{
			if (orgId != null && orgId > 0)
			{
				RegionManagers1cGuidsViewModel viewModel = _accountService.GetRegionManagers1cGuidsByOrg(orgId);
				return PartialView("_RegionManagers1cGuidsPartialForm", viewModel);
			}

			return null;
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private async Task SignInAsync(OrderPortalUser user, bool isPersistent)
		{
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
			AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null)
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}