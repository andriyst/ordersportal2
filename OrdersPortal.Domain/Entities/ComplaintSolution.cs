using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class ComplaintSolution
    {
        [Key]
        public int ComplaintSolutionId { get; set; }
        public int? ComplaintIssueId { get; set; }
        public string SolutionGuid { get; set; }
        public string SolutionText { get; set; }
        public bool Enabled { get; set; }
        public virtual ComplaintIssue ComplaintIssue { get; set; }
    }
}