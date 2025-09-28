using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class ComplaintDecision
    {
        [Key]
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }
        public DateTime CreateDate { get; set; }
        public int ComplaintIssueId { get; set; }
        public virtual ComplaintIssue ComplaintIssue { get; set; }
        public int ComplaintSolutionId { get; set; }
        public virtual ComplaintSolution ComplaintSolution { get; set; }
        public bool CustomerApprove { get; set; }
        public bool ManagerApprove { get; set; }
        public DateTime? FinalAppoveDate { get; set; }
		public string Description { get; set; }

    }
}