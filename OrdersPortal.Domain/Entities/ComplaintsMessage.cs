using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersPortal.Domain.Entities
{
    public class ComplaintsMessage
    {
        [Key]
        public int СomplaintsMessageId { get; set; }

        //--------------------------
        public string MessageWriterId { get; set; }
        public OrderPortalUser MessageWriter { get; set; }

        //---------------------------
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime MessageTime { get; set; }

        //---------------------------
        [Display(Name = "Повідомлення")]
        public string Message { get; set; }

        //---------------------------
        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }

    }
}