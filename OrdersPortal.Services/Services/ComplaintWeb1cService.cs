//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;

//using OrdersPortal.Domain.Dto;
//using OrdersPortal.Domain.Entities;
//using OrdersPortal.Infrastructure.EntityFramework;
//using OrdersPortal.Services.Connected_Services.WebPortalComplaints1cService;
//using OrdersPortal.Services.WebPortalComplaints1cService;

//namespace OrdersPortal.Services.Services
//{
//	public class ComplaintWeb1cService
//	{
//		private OrderPortalDbContext _datacontext;
//		private readonly complaintsPortTypeClient _complaintService;
//		//private readonly OrderRepository _orderRepository;

//		//private const string ComplaintsEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/ws_complaints.1cws";
//		private const string ComplaintsEndPointAddress = "http://192.168.1.10/oknastyle/ws/ws_complaints.1cws";


//		public ComplaintWeb1cService(OrderPortalDbContext datacontext)
//		{
//			_datacontext = datacontext;
//			//_orderRepository = new OrderRepository(_datacontext);

//			BasicHttpBinding myBinding = new BasicHttpBinding();

//			myBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
//			myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
//			myBinding.MaxReceivedMessageSize = 2147483647;
//			myBinding.MaxBufferSize = 2147483647;
//			myBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);

//			EndpointAddress ea = new EndpointAddress(@ComplaintsEndPointAddress);
//			_complaintService = new complaintsPortTypeClient(myBinding, ea);

//			_complaintService.ClientCredentials.UserName.UserName = "webuser";
//			_complaintService.ClientCredentials.UserName.Password = "webP@ssw0rd2014";

//		}


//		public bool CheckOrderInPeriodService(string code, string orderNumber, DateTime startDate)
//		{
			
//			return _complaintService.check_kontr_order(orderNumber, code, startDate); 
//		}

//		public bool LoadIssueAndSolutionList()
//		{

//			var res = _complaintService.getComplaintIssuesList();

//			//var res = service.getComplaintSolutionsListByIssue("111");


//			var allIssues = _datacontext.ComplaintIssues.ToList();
//			allIssues.Select(c => { c.Enabled = false; return c; }).ToList();
//			bool checkresult = false;

//			if (res > 0)
//			{
//				foreach (var item in res.)
//				{
//					ComplaintIssue updatedIssue = allIssues.FirstOrDefault(x => x.IssueGuid == item.GUID);

//					if (updatedIssue != null)
//					{
//						updatedIssue.Enabled = true;
//						updatedIssue.IssueText = item.Name;
//					}
//					else
//					{
//						ComplaintIssue newIssue = new ComplaintIssue()
//						{
//							Enabled = true,
//							IssueText = item.Name,
//							IssueGuid = item.GUID
//						};

//						_datacontext.ComplaintIssues.Add(newIssue);
//					}



//				}

//				_datacontext.SaveChanges();
//				checkresult = true;
//			}

//			foreach (var item in _datacontext.ComplaintIssues.Where(x => x.Enabled))
//			{


//				var issueSolutions = _datacontext.ComplaintSolutions.Where(x => x.ComplaintIssue.IssueGuid == item.IssueGuid).ToList();
//				issueSolutions.Select(c =>
//				{
//					c.Enabled = false;
//					return c;
//				}).ToList();

//				var resSolutions = _complaintService.getComplaintSolutionsListByIssue(item.IssueGuid);

//				if (resSolutions.Count > 0)
//				{
//					foreach (var sol in resSolutions)
//					{
//						ComplaintSolution updatedSolution = issueSolutions.FirstOrDefault(x => x.SolutionGuid == sol.GUID);

//						if (updatedSolution != null)
//						{
//							updatedSolution.Enabled = true;
//							updatedSolution.SolutionText = sol.Name;
//							updatedSolution.ComplaintIssue = _datacontext.ComplaintIssues.FirstOrDefault(x => x.IssueGuid == item.IssueGuid);
//						}
//						else
//						{
//							ComplaintSolution newSolution = new ComplaintSolution()
//							{
//								Enabled = true,
//								SolutionText = sol.Name,
//								SolutionGuid = sol.GUID,
//								ComplaintIssue = _datacontext.ComplaintIssues.FirstOrDefault(x => x.IssueGuid == item.IssueGuid)
//							};

//							_datacontext.ComplaintSolutions.Add(newSolution);
//						}

//					}

//				}
//			}
//			_datacontext.SaveChanges();

//			return checkresult;

//		}

//		public void ApproveComplaint(int complaintId)
//		{
//			var complaint = _datacontext.Complaints.FirstOrDefault(x => x.ComplaintId == complaintId);
//			if (complaint != null)
//			{
//				complaint.StatusId = 18;
//				var lastDecision = complaint.ComplaintDecisions.OrderByDescending(x => x.Id).FirstOrDefault();
//				if (lastDecision != null)
//					lastDecision.CustomerApprove = true;
//			}

//			_datacontext.SaveChanges();
//		}

//		public List<ComplaintOrderSerieDto> GetComplaintOrderSeriesByOrderNumber(string orderNumber, int orderId)
//		{
//			List<ComplaintOrderSerieDto> result = new List<ComplaintOrderSerieDto>();

//			var res = _complaintService.getComplaintSeries(orderNumber);
			
//			foreach (var item in res)
//			{
//				result.Add(new ComplaintOrderSerieDto
//				{
//					SerieGuid = item.GUID,
//					SerieName = item.Name,
//					SerieCategory = item.ParentName,
//					OrderId = orderId 
//				});

//			}

//			return result;

//		}

//	}
//}