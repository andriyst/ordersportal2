
using NLog;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Application.Services;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrdersPortal.WebUI.Controllers
{
    public class OrganizationController : Controller
    {
		private readonly IOrganizationRepository _organizationRepository;
		private readonly IOrganizationService _organizationService;
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


		public OrganizationController(IOrganizationRepository organizationRepository, IOrganizationService organizationService)
		{
			_organizationRepository = organizationRepository;
			_organizationService = organizationService;
		}
		// GET: Organization
		public ActionResult List()
		{
			var model = _organizationService.GetOrganizationList();
			return View(model);
		}


	

		public ActionResult Add()
		{
			OrganizationAddViewModel  viewModel = _organizationService.PrepareAddVierwModel(new OrganizationAddViewModel());
			return View(viewModel);
		}

	
		[HttpPost]
		[ValidateAntiForgeryToken]

		public ActionResult Add(OrganizationAddViewModel model)
		{
			if (ModelState.IsValid)
			{
				Organization organization = new Organization
				{
					OrganizationName = model.OrganizationName,
					Organization1cId = model.Organization1cId				

				};
				_organizationService.AddOrganization(organization);

			}

			return RedirectToAction("List");

		}

	
		public ActionResult Edit(int organizationId)
		{
			Organization organizatio = _organizationRepository.GetById(organizationId);
			return View(organizatio);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Organization organization)
		{
			_organizationService.EditOrganization(organization);

			return RedirectToAction("List");
		}

		public ActionResult Del(int organizationId)
		{
			try
			{
				_organizationService.RemoveOrganization(organizationId);
			}
			catch (Exception ex)
			{
				_logger.Error(ex);
			}

			return RedirectToAction("List");

		}
	}
}