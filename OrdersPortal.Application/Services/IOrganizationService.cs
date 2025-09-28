using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Services
{
	public interface IOrganizationService
	{
		List<Organization> GetOrganizationList();
		void AddOrganization(Organization organization);
		void RemoveOrganization(int organizationId);
		void EditOrganization(Organization organization);
		OrganizationAddViewModel PrepareAddVierwModel(OrganizationAddViewModel viewModel);
	}
}
