using System.Collections.Generic;
using System.Web.Mvc;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Dto;

namespace OrdersPortal.Application.Services
{
	public interface IComplaintsService
	{
		UploadComplaintViewModel PrepareUploadComplaintViewModel();
		void UploadComplaint(UploadComplaintViewModel model);
		EditComplaintViewModel EditComplaint(int complaintId);
		void SaveComplaint(EditComplaintViewModel complaint);
		ComplaintDetailsViewModel GetComplaintDetailsByComplaintId(int complaintId);
		byte[] GetPhotoById(int photoId);
		List<ComplaintOrderSerieDto> GetComplaintOrderSeriesByOrderNumber(string orderNumber);
		List<SelectListItem> GetSolutionsByIssue(int issueId);
		void ApproveComplaint(int complaintId);
		bool LoadIssueAndSolutionList();
		bool CheckContrAgentOrder(string orderNumber);
	}
}
