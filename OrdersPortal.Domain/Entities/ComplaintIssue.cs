using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class ComplaintIssue
    {
        [Key]
        public int ComplaintIssueId { get; set; }
        public string IssueText { get; set; }
        public string IssueGuid { get; set; }
        public bool Enabled { get;set;}
      
    }
}