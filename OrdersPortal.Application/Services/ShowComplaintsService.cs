using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using OrdersPortal.Application.Models.ViewModels;

using OrdersPortal.Domain.Entities;
using OrdersPortal.Infrastructure.EntityFramework;

namespace OrdersPortal.Application.Services
{
    public class ShowComplaintsService
    {
        private OrderPortalDbContext _datacontext;
        private UserManager<OrderPortalUser> _userManager;
       

        public ShowComplaintsService(OrderPortalDbContext datacontext)
        {
            _datacontext = datacontext;
            _userManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(_datacontext));
           
        }

        public List<UploadComplaintsListViewModel> GetCustomerComplaints(string customerId, List<int> statuses)
        {
            List<UploadComplaintsListViewModel> complaintslist = new List<UploadComplaintsListViewModel>();

            List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();


            List<int> StatusId = new List<int>();
            if (statuses != null)
            {
                StatusId.AddRange(statuses);
            }
            else
            {
                StatusId.AddRange(AllStatuses);

            }

            OrdersMessage EmptyLastMessage = new OrdersMessage()
            {
                Message = "Немає Повідомлень"
            };


            complaintslist = _datacontext.Complaints
                   .Where(x => x.CustomerId == customerId)
                   .Where(x => StatusId.Contains(x.StatusId))
                   .Select(d => new UploadComplaintsListViewModel
                   {
                       CustomerName = d.Customer.FullName,
                       ComplaintOrderNumber = d.ComplaintOrderNumber,
                      // ComplaintSolution = d.ComplaintSolution,
                       ManagerName = d.Manager.FullName,
                       ComplaintId = d.ComplaintId,
                       ComplaintIssue = d.ComplaintDecisions.OrderByDescending(x=>x.Id).FirstOrDefault().ComplaintIssue.IssueText,
                       ComplaintSolution = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintSolution.SolutionText,
                       ComplaintDescription = d.ComplaintDescription,
	                   ApproveDate = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().FinalAppoveDate,
	                      
                       ComplaintDate = d.ComplaintDate,
                       ComplaintOrderDefineDate = d.ComplaintOrderDefineDate,
                       ComplaintOrderDeliverDate = d.ComplaintOrderDeliverDate,
                       StatusName = d.Status.StatusName
                   })
                   .OrderByDescending(x => x.ComplaintId)
                   .ToList();

	        foreach (var complaint in complaintslist)
	        {
		        if (complaint.ApproveDate != null && complaint.ApproveDate > complaint.ComplaintDate)
		        {
			        complaint.ComplaintDescription = complaint.ComplaintDescription + " <span style='color: orange'><b>Гранична дата повернення на обмін " + complaint.ApproveDate.Value.ToShortDateString() + "р.</b></span>";
				}
	        }


            return complaintslist;
        }

	  
        public List<UploadComplaintsListViewModel> GetManagerComplaints(string managerId, List<int> statuses)
        {
            List<UploadComplaintsListViewModel> complaintslist = new List<UploadComplaintsListViewModel>();

            List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();

            List<int> StatusId = new List<int>();
            if (statuses != null)
            {
                StatusId.AddRange(statuses);
            }
            else
            {
                StatusId.AddRange(AllStatuses);

            }

            OrdersMessage EmptyLastMessage = new OrdersMessage()
            {
                Message = "Немає Повідомлень"
            };

            complaintslist = _datacontext.Complaints
                   .Where(x => x.ManagerId == managerId)
                     .Where(x => StatusId.Contains(x.StatusId))
                   .Select(d => new UploadComplaintsListViewModel
                   {
                       CustomerName = d.Customer.FullName,
                       ComplaintOrderNumber = d.ComplaintOrderNumber,
                      // ComplaintSolution = d.ComplaintSolution,
                       ManagerName = d.Manager.FullName,
                       ComplaintId = d.ComplaintId,
                       ComplaintIssue = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintIssue.IssueText,
                       ComplaintSolution = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintSolution.SolutionText,
					   ComplaintDescription = d.ComplaintDescription,
	                   ApproveDate = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().FinalAppoveDate,

					   ComplaintDate = d.ComplaintDate,
                       ComplaintOrderDefineDate = d.ComplaintOrderDefineDate,
                       ComplaintOrderDeliverDate = d.ComplaintOrderDeliverDate,
                       StatusName = d.Status.StatusName
                   })
                   .OrderByDescending(x=>x.ComplaintId)
                   .ToList();

	        foreach (var complaint in complaintslist)
	        {
		        if (complaint.ApproveDate != null && complaint.ApproveDate > complaint.ComplaintDate)
		        {
					complaint.ComplaintDescription = complaint.ComplaintDescription + " <span style='color: orange'><b>Гранична дата повернення на обмін " + complaint.ApproveDate.Value.ToShortDateString() + "р.</b></span>";
				}
	        }

			return complaintslist;

        }
        public List<UploadComplaintsListViewModel> GetRegionManagerComplaints(string regionmanagerId, List<int> statuses)
        {
            List<UploadComplaintsListViewModel> complaintslist = new List<UploadComplaintsListViewModel>();
            //var man = _repo.GetManagersByRegionManagerId(regionmanagerId).Select(x => x.Id);

            List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();

            List<int> StatusId = new List<int>();
            if (statuses != null)
            {
                StatusId.AddRange(statuses);
            }
            else
            {
                StatusId.AddRange(AllStatuses);

            }

            OrdersMessage EmptyLastMessage = new OrdersMessage()
            {
                Message = "Немає Повідомлень"
            };


            complaintslist = _datacontext.Complaints
                               .Where(x => x.Customer.Customer.RegionManagerId == regionmanagerId)
                               .Where(x => StatusId.Contains(x.StatusId))
                                 .Select(d => new UploadComplaintsListViewModel
                                 {
                                     CustomerName = d.Customer.FullName,
                                     ComplaintOrderNumber = d.ComplaintOrderNumber,
                                     //ComplaintSolution = d.ComplaintSolution,
                                     ManagerName = d.Manager.FullName,
                                     ComplaintId = d.ComplaintId,
                                     ComplaintIssue = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintIssue.IssueText,
                                     ComplaintSolution = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintSolution.SolutionText,
									 ComplaintDescription = d.ComplaintDescription,
	                                 ApproveDate = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().FinalAppoveDate,

									 ComplaintDate = d.ComplaintDate,
                                     ComplaintOrderDefineDate = d.ComplaintOrderDefineDate,
                                     ComplaintOrderDeliverDate = d.ComplaintOrderDeliverDate,
                                     StatusName = d.Status.StatusName
                                 })
                                 .OrderByDescending(x => x.ComplaintId)
                                 .ToList();
	        foreach (var complaint in complaintslist)
	        {
		        if (complaint.ApproveDate != null && complaint.ApproveDate > complaint.ComplaintDate)
		        {
					complaint.ComplaintDescription = complaint.ComplaintDescription + " <span style='color: orange'><b>Гранична дата повернення на обмін " + complaint.ApproveDate.Value.ToShortDateString() + "р.</b></span>";
				}
	        }


			return complaintslist;

        }
        public List<UploadComplaintsListViewModel> GetAllComplaints(List<int> statuses)
        {
            List<UploadComplaintsListViewModel> complaintslist = new List<UploadComplaintsListViewModel>();


            List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();

            List<int> StatusId = new List<int>();
            if (statuses != null)
            {
                StatusId.AddRange(statuses);
            }
            else
            {
                StatusId.AddRange(AllStatuses);

            }

            OrdersMessage EmptyLastMessage = new OrdersMessage()
            {
                Message = "Немає Повідомлень"
            };


            List<UploadComplaintsListViewModel> UserComplaintsList = _datacontext.Complaints
                                 .Where(x => StatusId.Contains(x.StatusId))
                                 .Select(d => new UploadComplaintsListViewModel
                                 {
                                     CustomerName = d.Customer.FullName,
                                     ComplaintOrderNumber = d.ComplaintOrderNumber,
                                     //= d.ComplaintDescription.FirstOrDefault().,
                                     ManagerName = d.Manager.FullName,
                                     ComplaintId = d.ComplaintId,
                                     ComplaintIssue = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintIssue.IssueText,
                                     ComplaintSolution = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().ComplaintSolution.SolutionText,
									 ComplaintDescription = d.ComplaintDescription,
	                                 ApproveDate = d.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault().FinalAppoveDate,

									 ComplaintDate = d.ComplaintDate,
                                     ComplaintOrderDefineDate = d.ComplaintOrderDefineDate,
                                     ComplaintOrderDeliverDate = d.ComplaintOrderDeliverDate,
                                     StatusName = d.Status.StatusName
                                 })
                                 .OrderByDescending(x => x.ComplaintId)
                                 .ToList();
	        foreach (var complaint in UserComplaintsList)
	        {
		        if (complaint.ApproveDate != null && complaint.ApproveDate > complaint.ComplaintDate)
		        {
			        complaint.ComplaintDescription = complaint.ComplaintDescription + " <span style='color: orange'><b>Гранична дата повернення на обмін " + complaint.ApproveDate.Value.ToShortDateString() + "р.</b></span>";
		        }
	        }

			return UserComplaintsList;
        }
    }
}