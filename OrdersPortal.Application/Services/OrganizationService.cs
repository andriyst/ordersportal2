using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly IOrganizationRepository _organizationRepository;
		private readonly ICustomersServices _customersServices;

		public OrganizationService(IOrganizationRepository organizationRepository, ICustomersServices customersServices)
		{
			_organizationRepository = organizationRepository;
			_customersServices = customersServices;
		}

		public List<Organization> GetOrganizationList()
		{
			return _organizationRepository.GetList();
		}

		public void AddOrganization(Organization organization)
		{
			_organizationRepository.AddPermanent(organization);
		}

		public void RemoveOrganization(int organizationId)
		{
			var organization = _organizationRepository.GetById(organizationId);
			if (organization != null)
			{
				_organizationRepository.Remove(organization);
				_organizationRepository.SaveChanges();
			}
		}

		public void EditOrganization(Organization organization)
		{
			var result = _organizationRepository.GetById(organization.OrganizationId);
			organization.OrganizationName = organization.OrganizationName;
			_organizationRepository.UpdatePermanent(result);
		}
		public OrganizationAddViewModel PrepareAddVierwModel(OrganizationAddViewModel viewModel)
		{
			var organization1cList = _customersServices.GetOrganizationList();
			viewModel.Organization1cList = organization1cList.Select(x => new Organization1c { Name = $"{x.Name} - ({x.Code  })", Code = x.Code }).ToList();
			return viewModel;
		}
	}
}
