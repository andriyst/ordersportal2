using System.Collections.Generic;
using OrdersPortal.Application.Models;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Services
{
	public interface IAccountService
	{
		OrderPortalUser GetById(string accountId);
		RegisterUserViewModel PrepareRegisterViewModel();
		void AddUser(RegisterUserViewModel model);
		List<UsersListViewModel> GetUserListByRole(string roleName);
		ChangeUserPasswordViewModel GetUserPassModelByName(string userName);
		void ChangeUserPassword(ChangeUserPasswordViewModel model);
		EditUserViewModel PrepareEditUserViewModel(string userName);
		void EditUser(EditUserViewModel model);
		bool UpdateManagers(Update1cManagersDto model);
		UserManagersViewModel GetUserManagers(string accountId);
		OrganizationCustomerCodesViewModel GetContrAgentCodesByOrg(int orgId);
		RegionManagersViewModel GetRegionManagersByOrg(int orgId);
		ManagersViewModel GetManagersByOrg(int orgId);
		RegionManagers1cGuidsViewModel GetRegionManagers1cGuidsByOrg(int orgId);
		Managers1cGuidsViewModel GetManagers1cGuidsByOrg(int orgId);
	}
}
