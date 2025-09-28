using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class ComplaintOrderSerie
    {
        [Key]
        public int Id { get; set; }
        public string SerieGuid { get; set; }
        public string SerieName { get; set; }
        public int? OrderId { get; set; }
        public int ComplaintId { get; set; }
        public virtual Complaint Complaint { get; set; }
    }
}