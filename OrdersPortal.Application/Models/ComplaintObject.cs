using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models
{
    public class ComplaintObject
    {

        public ComplaintObject()
		{
            this.Complaint = new HashSet<Complaint>();
		}
        [Key]
        public int ComplaintObjectId { get; set; }
        public string ComplaintObjectName { get; set; }

        public virtual ICollection<Complaint> Complaint { get; set; }
        
    }
}