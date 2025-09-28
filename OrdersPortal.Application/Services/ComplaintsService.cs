using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;

namespace OrdersPortal.Application.Services
{
	public class ComplaintsService : IComplaintsService
	{
		private readonly IComplaintsRepository _complaintsRepository;
	
		private readonly IComplaintIssueRepository _complaintIssueRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly IComplaintSolutionRepository _complaintSolutionRepository;
		private readonly IComplaintOrderSeriesRepository _complaintOrderSeriesRepository;
		private readonly IComplaintPhotoRepository _complaintPhotoRepository;
		private readonly IAccountService _accountService;
		private readonly ICustomersServices _customersServices;
		private readonly ApplicationContext _applicationContext;

		private const int InitComplaintStatusId = 1;

		public ComplaintsService(IComplaintsRepository complaintsRepository, 
			IComplaintIssueRepository complaintIssueRepository, IOrderRepository orderRepository, IComplaintOrderSeriesRepository complaintOrderSeriesRepository,
			IComplaintPhotoRepository complaintPhotoRepository,
			IComplaintSolutionRepository complaintSolutionRepository, IAccountService accountService, ICustomersServices customersServices, ApplicationContext applicationContext)
		{
			_complaintsRepository = complaintsRepository;
			
			_complaintIssueRepository = complaintIssueRepository;
			_orderRepository = orderRepository;
			_complaintSolutionRepository = complaintSolutionRepository;
			_complaintOrderSeriesRepository = complaintOrderSeriesRepository;
			_complaintPhotoRepository = complaintPhotoRepository;
			_accountService = accountService;
			_customersServices = customersServices;
			_applicationContext = applicationContext;
		}

		public UploadComplaintViewModel PrepareUploadComplaintViewModel()
		{
			OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);
			UploadComplaintViewModel model = new UploadComplaintViewModel();

			model.CustomerName = currentUser.FullName;
			model.ComplaintIssues = _complaintIssueRepository.GetAll().ToList()
														  .Select(x =>
															  new SelectListItem
															  {
																  Text = x.IssueText,
																  Value = x.ComplaintIssueId.ToString()
															  })
														  .ToList();
			model.ComplaintIssues.Insert(0, new SelectListItem { Text = "Виберіть причину", Value = "" });

			model.ComplaintSolutions = new List<SelectListItem>();
			model.ComplaintsOrderSeries = new List<ComplaintsOrderSerieViewModel>();

			return model;
		}

		public void UploadComplaint(UploadComplaintViewModel model)
		{
			OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);

			List<ComplaintPhoto> complaintPhotos = new List<ComplaintPhoto>();

			foreach (var file in model.File)
			{
				if (file != null)
				{
					byte[] imageData;

					using (var binaryReader = new BinaryReader(file.InputStream))
					{
						imageData = binaryReader.ReadBytes(file.ContentLength);
					}

					ComplaintPhoto tmpPhoto = new ComplaintPhoto();
					tmpPhoto.Photo = imageData;
					tmpPhoto.PhotoName = file.FileName;
					tmpPhoto.PhotoSize = imageData.Count();
					tmpPhoto.AddThumbnail();
					complaintPhotos.Add(tmpPhoto);
				}
			}

			Complaint tmpCompl = new Complaint
			{
				ComplaintOrderDefineDate = model.ComplaintOrderDefineDate ?? DateTime.Now,
				ComplaintOrderDeliverDate = model.ComplaintOrderDeliverDate ?? DateTime.Now,
				CustomerId = currentUser.Id,
				ManagerId = currentUser.Customer.ManagerId,
				ComplaintOrderNumber = model.ComplaintOrderNumber,
				StatusId = InitComplaintStatusId,
				ComplaintDescription = model.ComplaintDescription,
				ComplaintDate = DateTime.Now,
				ComplaintWorkStartDate = DateTime.Now,
				ComplaintCompleteDate = DateTime.Now,
				ComplaintPhoto = complaintPhotos,
				ComplaintOrderSeries = new List<ComplaintOrderSerie>()
			};

			ComplaintDecision tempCompDecision = new ComplaintDecision
			{
				ComplaintIssue = _complaintIssueRepository.GetById(model.ComplaintIssue),
				ComplaintSolution = _complaintSolutionRepository.GetById(model.ComplaintSolution),
				ManagerApprove = false,
				CustomerApprove = true,
				CreateDate = DateTime.Now
			};

			tmpCompl.ComplaintDecisions = new List<ComplaintDecision> { tempCompDecision };

			if (model.ComplaintsOrderSeries != null)
			{
				foreach (var item in model.ComplaintsOrderSeries)
				{
					if (item.Checked)
					{
						tmpCompl.ComplaintOrderSeries.Add(new ComplaintOrderSerie
						{
							SerieGuid = item.SerieGuid,
							SerieName = item.SerieName,
							OrderId = item.OrderId
						});
					}
				}
			}

			_complaintsRepository.AddPermanent(tmpCompl);
		}

		public EditComplaintViewModel EditComplaint(int complaintId)
		{
			Complaint complaint = _complaintsRepository.GetById(complaintId);

			if (complaint != null && (complaint.Status.StatusId == 6 || complaint.Status.StatusId == 8 || complaint.Status.StatusId == 7))
			{
				return null;
			}

			EditComplaintViewModel editComplaint = new EditComplaintViewModel();

			if (complaint != null)
			{

				editComplaint.ComplaintDate = complaint.ComplaintDate;
				editComplaint.ComplaintDescription = complaint.ComplaintDescription; //complaint.ComplaintsMessage.Where(x=>x.MessageWriterId == complaint.CustomerId).OrderByDescending(x=>x.MessageWriterId).FirstOrDefault()?.Message;
				editComplaint.ComplaintId = complaint.ComplaintId;
				editComplaint.ComplaintOrderDefineDate = complaint.ComplaintOrderDefineDate;
				editComplaint.ComplaintOrderDeliverDate = complaint.ComplaintOrderDeliverDate;
				editComplaint.ComplaintOrderNumber = complaint.ComplaintOrderNumber;
				editComplaint.CustomerName = complaint.Customer.FullName;
				editComplaint.ManagerName = complaint.Manager.FullName;

				editComplaint.ComplaintPhotoThubmnails = complaint.ComplaintPhoto
																  .Where(y => y.ComplaintId == complaintId)
																  .Select(y => new ComplaintDetailPhotoViewModel
																  {
																	  ComplaintPhotoThumbnail = y.PhotoIco,
																	  ComplaintPhotoId = y.ComplaintPhotoId
																  })
																  .ToList();

				editComplaint.ComplaintIssues = _complaintIssueRepository.GetAll()
														 .Select(x =>
															 new SelectListItem
															 {
																 Text = x.IssueText,
																 Value = x.ComplaintIssueId.ToString()
															 })
														 .ToList();
				editComplaint.ComplaintIssues.Insert(0, new SelectListItem { Text = "Виберіть причину", Value = "" });
				editComplaint.ComplaintSolutions = new List<SelectListItem>();
				editComplaint.ComplaintDecisions = complaint.ComplaintDecisions;

				var orderId = _orderRepository.GetOrderByDb1cOrderNumber(complaint.ComplaintOrderNumber).OrderId;
				editComplaint.ComplaintsOrderSeries = _customersServices.GetComplaintOrderSeriesByOrderNumber(complaint.ComplaintOrderNumber, orderId);

				foreach (var item in editComplaint.ComplaintsOrderSeries)
				{
					if (complaint.ComplaintOrderSeries.Select(x => x.SerieGuid).Contains(item.SerieGuid))
					{
						item.Checked = true;
					}
				}

				return editComplaint;
			}

			return null;
		}

		public void SaveComplaint(EditComplaintViewModel complaint)
		{
			Complaint tmpCompl = _complaintsRepository.GetById(complaint.ComplaintId);
			
			List<ComplaintPhoto> complaintPhotos = new List<ComplaintPhoto>();

			foreach (var file in complaint.File)
			{
				if (file != null)
				{
					byte[] imageData;
					// считываем переданный файл в массив байтов
					using (var binaryReader = new BinaryReader(file.InputStream))
					{
						imageData = binaryReader.ReadBytes(file.ContentLength);
					}
					// установка массива байтов
					ComplaintPhoto tmpPhoto = new ComplaintPhoto
					{
						Photo = imageData,
						PhotoName = file.FileName,
						PhotoSize = imageData.Count()
					};

					tmpPhoto.AddThumbnail();

					complaintPhotos.Add(tmpPhoto);


				}
			}


			tmpCompl.ComplaintWorkStartDate = DateTime.Today;
			tmpCompl.ComplaintCompleteDate = DateTime.Today;
			tmpCompl.ComplaintPhoto = complaintPhotos;


			ComplaintDecision tempCompDecision = new ComplaintDecision
			{
				ComplaintIssue = _complaintIssueRepository.GetById(complaint.ComplaintIssue),
				ComplaintSolution = _complaintSolutionRepository.GetById(complaint.ComplaintSolution),
				ManagerApprove = false,
				CustomerApprove = true,
				CreateDate = DateTime.Now,
				Description = complaint.ComplaintDescription
			};


			_complaintOrderSeriesRepository.RemoveByComplaintId(tmpCompl.ComplaintId);



			if (complaint.ComplaintsOrderSeries != null)
			{
				int? orderId = _orderRepository.GetOrderByDb1cOrderNumber(complaint.ComplaintOrderNumber)?.OrderId;
				foreach (var item in complaint.ComplaintsOrderSeries)
				{
					if (item.Checked)
					{
						tmpCompl.ComplaintOrderSeries.Add(new ComplaintOrderSerie
						{
							SerieGuid = item.SerieGuid,
							SerieName = item.SerieName,
							OrderId = orderId ?? 0
						});
					}
				}
			}

			tmpCompl.ComplaintDecisions.Add(tempCompDecision);

			_complaintsRepository.SaveChanges();

		}

		public ComplaintDetailsViewModel GetComplaintDetailsByComplaintId(int complaintId)
		{
			var model = _complaintsRepository.GetByIdWithIncludes(complaintId);

			var result = new ComplaintDetailsViewModel
			{
				ComplaintId = model.ComplaintId,
				ComplaintDate = model.ComplaintDate,
				ComplaintOrderNumber = model.ComplaintOrderNumber,
				CustomerName = model.Customer.FullName,
				ManagerName = model.Manager.FullName,
				ComplaintDescription = model.ComplaintDecisions.OrderByDescending(y => y.Id).FirstOrDefault()?.Description,
				ComplaintIssue = model.ComplaintDecisions.OrderByDescending(y => y.Id).FirstOrDefault()?.ComplaintIssue.IssueText,
				ComplaintSolution = model.ComplaintDecisions.OrderByDescending(y => y.Id).FirstOrDefault()?.ComplaintSolution.SolutionText,
				ManagerApprove = model.ComplaintDecisions?.OrderByDescending(y => y.Id).FirstOrDefault()?.ManagerApprove ?? false,
				CustomerApprove = model.ComplaintDecisions?.OrderByDescending(y => y.Id).FirstOrDefault()?.CustomerApprove ?? false,
				ComplaintOrderDeliverDate = model.ComplaintOrderDeliverDate,
				ComplaintOrderDefineDate = model.ComplaintOrderDefineDate,
				ComplaintWorkStartDate = model.ComplaintWorkStartDate,
				Complaint1cOrder = model.Complaint1cOrder,
				ComplaintActNumber = model.ComplaintActNumber,
				ComplaintResult = model.ComplaintResult,
				FinalApproveDate = model.ComplaintDecisions?.OrderByDescending(y => y.Id).FirstOrDefault()?.FinalAppoveDate,

				ComplaintsOrderSeries = model.ComplaintOrderSeries.Select(z => new ComplaintsOrderSerieViewModel
				{
					SerieGuid = z.SerieGuid,
					SerieName = z.SerieName,
					OrderId = z.OrderId ?? 0
				}).ToList(),
				ComplaintPhotoThubmnails = model.ComplaintPhoto
					.Where(y => y.ComplaintId == complaintId)
					.Select(y => new ComplaintDetailPhotoViewModel
					{
						ComplaintPhotoThumbnail = y.PhotoIco,
						ComplaintPhotoId = y.ComplaintPhotoId
					})
					 .ToList(),
				StatusName = model.Status.StatusName

			};

			if (result.FinalApproveDate != null && result.FinalApproveDate > result.ComplaintDate)
			{
				result.ComplaintDescription = result.ComplaintDescription + " <span style='color: orange'><b>Гранична дата повернення на обмін "
																		  + result.FinalApproveDate.Value.ToShortDateString() + "р.</b></span>";
			}

			return result;
		}

		public byte[] GetPhotoById(int photoId)
		{
			return _complaintPhotoRepository.GetById(photoId)?.Photo;
		}

		public List<ComplaintOrderSerieDto> GetComplaintOrderSeriesByOrderNumber(string orderNumber)
		{
			var order = _orderRepository.GetOrderByDb1cOrderNumber(orderNumber);
			if (order != null)
			{
				return _customersServices.GetComplaintOrderSeriesByOrderNumber(orderNumber, order.OrderId); 
			}

			return null;
		}

		public List<SelectListItem> GetSolutionsByIssue(int issueId)
		{
			var solutions = _complaintSolutionRepository.GetComplaintSolutionByIssueId(issueId);
			var result = solutions.Select(x => new SelectListItem
			{
				Value = x.ComplaintSolutionId.ToString(),
				Text = x.SolutionText

			}).ToList();
			result.Insert(0, new SelectListItem { Text = "Виберіть рішення", Value = "" });

			return result;
		}

		public void ApproveComplaint(int complaintId)
		{

			var complaint = _complaintsRepository.GetByIdWithDecision(complaintId);
			if (complaint != null)
			{
				complaint.StatusId = 18;
				var lastDecision = complaint.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault();
				if (lastDecision != null)
					lastDecision.CustomerApprove = true;
			}

			_complaintsRepository.SaveChanges();

		}

		public bool LoadIssueAndSolutionList()
		{
			List<ComplaintIssueDto> issuesGetList = _customersServices.LoadIssueAndSolutionList();


			var allIssues = _complaintIssueRepository.GetAll().ToList();

			//allIssues.Select(c => { c.Enabled = false; return c; }).ToList();
			bool checkResult = false;

			if (issuesGetList.Count > 0)
			{
				foreach (var item in issuesGetList)
				{
					ComplaintIssue updatedIssue = allIssues.FirstOrDefault(x => x.IssueGuid == item.Guid);

					if (updatedIssue != null)
					{
						updatedIssue.Enabled = true;
						updatedIssue.IssueText = item.Name;
					}
					else
					{
						ComplaintIssue newIssue = new ComplaintIssue
						{
							Enabled = true,
							IssueText = item.Name,
							IssueGuid = item.Guid
						};

						_complaintIssueRepository.Add(newIssue);
					}



				}

				_complaintIssueRepository.SaveChanges();

				checkResult = true;
			}

			foreach (var item in _complaintIssueRepository.GetAll().Where(x => x.Enabled))
			{


				var issueSolutions = _complaintSolutionRepository.GetAll().Where(x => x.ComplaintIssue.IssueGuid == item.IssueGuid).ToList();

				//issueSolutions.Select(c =>
				//{
				//	c.Enabled = false;
				//	return c;
				//}).ToList();

				var resSolutions = _customersServices.GetComplaintSolutionsListByIssue(item.IssueGuid);

				if (resSolutions.Count > 0)
				{
					foreach (var sol in resSolutions)
					{
						ComplaintSolution updatedSolution = issueSolutions.FirstOrDefault(x => x.SolutionGuid == sol.Guid);

						if (updatedSolution != null)
						{
							updatedSolution.Enabled = true;
							updatedSolution.SolutionText = sol.Name;
							updatedSolution.ComplaintIssue = _complaintIssueRepository.GetByGuid(item.IssueGuid);
						}
						else
						{
							ComplaintSolution newSolution = new ComplaintSolution
							{
								Enabled = true,
								SolutionText = sol.Name,
								SolutionGuid = sol.Guid,
								ComplaintIssue = _complaintIssueRepository.GetByGuid(item.IssueGuid)
							};

							_complaintSolutionRepository.Add(newSolution);
						}

					}

				}
			}
			_complaintSolutionRepository.SaveChanges();

			return checkResult;

		}

		public bool CheckContrAgentOrder(string orderNumber)
		{
			OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);

			try
			{
				var kod = currentUser.Customer?.CustomerContrCode;

				DateTime date = DateTime.Today.AddYears(-6);

				return _customersServices.CheckOrderInPeriodService(kod, orderNumber, date);
			}
			catch (Exception ex)
			{
				ex = ex;
				return false;
			}
		}
	}
}
