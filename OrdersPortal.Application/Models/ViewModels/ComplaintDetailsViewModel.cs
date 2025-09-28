using System;
using System.Collections.Generic;

namespace OrdersPortal.Application.Models.ViewModels
{
    public class ComplaintDetailsViewModel
    {
        public int ComplaintId { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string ComplaintOrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string ManagerName { get; set; }
        public string ComplaintDescription { get; set; }
        public string ComplaintIssue { get; set; }
        public string ComplaintSolution { get; set; }
        public DateTime ComplaintOrderDeliverDate { get; set; }
        public DateTime ComplaintWorkStartDate { get; set; }

        public DateTime ComplaintOrderDefineDate { get; set; }
        public string Complaint1cOrder { get; set; }
        public string ComplaintActNumber { get; set; }
        public string ComplaintResult { get; set; }

        public List<ComplaintsOrderSerieViewModel> ComplaintsOrderSeries { get; set; }

        public List<ComplaintDetailPhotoViewModel> ComplaintPhotoThubmnails { get; set; }
        public string StatusName { get; set; }
        public bool ManagerApprove { get; set; }
        public bool CustomerApprove { get; set; }
        public DateTime? FinalApproveDate { get; set; }

    }
}