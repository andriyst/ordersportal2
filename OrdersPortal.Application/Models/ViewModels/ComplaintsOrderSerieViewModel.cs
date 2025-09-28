
namespace OrdersPortal.Application.Models.ViewModels
{
    public class ComplaintsOrderSerieViewModel
    {
        public int Id { get; set; }
        public string SerieGuid { get; set; }
        public string SerieName { get; set; }
        public string SerieCategory { get; set; }
        public bool Checked { get; set; }
        public int ComplaintId { get; set; }
        public int OrderId { get; set; }
    }
  
}