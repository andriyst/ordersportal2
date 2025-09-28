using System;
using System.Collections.Generic;
using System.Linq;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class ComplaintListViewModel
	{
		public int ComplaintId { get; set; }
		public string ComplaintDate { get; set; }
		public DateTime ComplaintOrderDeliverDate { get; set; }
		public DateTime ComplaintOrderDefineDate { get; set; }
		public string ComplaintOrderNumber { get; set; }
		public string ManagerName { get; set; }
		public string CustomerName { get; set; }
		public string ComplaintDescription { get; set; }
		public string ComplaintActNumber { get; set; }
		public string ComplaintIssue { get; set; }
		public string ComplaintSolution { get; set; }
		public string Complaint1cOrder { get; set; }
		public string StatusName { get; set; }
		public string OrganizationName { get; set; }

		public string LastMessage { get; set; }
		public string LastMessageWriter { get; set; }
		public DateTime LastMessageTime { get; set; }
		public DateTime? ApproveDate { get; set; }

		public static IList<ComplaintListViewModel> ConvertFromEntities(IList<Complaint> entities)
		{
			return entities.Select(ConvertFromEntity).ToList();
		}

		public static ComplaintListViewModel ConvertFromEntity(Complaint entity)
		{
			var approveDate = entity.ComplaintDecisions?.OrderByDescending(x => x.Id).FirstOrDefault()?.FinalAppoveDate;

			ComplaintListViewModel result = new ComplaintListViewModel
			{
				ComplaintId = entity.ComplaintId,
				ComplaintDate = entity.ComplaintDate.ToString("dd.MM.yyyy HH:mm"),
				Complaint1cOrder = entity.Complaint1cOrder,
				ComplaintActNumber = entity.ComplaintActNumber,
				//ComplaintDescription = entity.ComplaintDescription,
				ComplaintDescription = (approveDate != null && approveDate > entity.ComplaintDate) ?
					entity.ComplaintDescription = entity.ComplaintDescription + " <span style='color: orange'><b>Гранична дата повернення на обмін " + approveDate.Value.ToShortDateString() + "р.</b></span>" :
					entity.ComplaintDescription,

				ComplaintIssue = entity.ComplaintDecisions?.OrderByDescending(x=>x.Id).FirstOrDefault()?.ComplaintIssue.IssueText,
				ComplaintSolution = entity.ComplaintDecisions?.OrderByDescending(x => x.Id).FirstOrDefault()?.ComplaintSolution.SolutionText,
				StatusName = entity.Status.StatusName,
				ComplaintOrderDefineDate = entity.ComplaintOrderDefineDate,
				ComplaintOrderDeliverDate = entity.ComplaintOrderDeliverDate,
				ComplaintOrderNumber = entity.ComplaintOrderNumber,
				OrganizationName = entity.Customer.OrderPortalUserOrganizations.FirstOrDefault().Organization.OrganizationName,
				CustomerName = entity.Customer.FullName,
				ManagerName = entity.Manager?.FullName ?? ""
			};

			return result;
		}
	}
}