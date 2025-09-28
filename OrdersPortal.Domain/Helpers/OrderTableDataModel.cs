
namespace OrdersPortal.Domain.Helpers
{
	public class OrderTableDataModel : TableDataModel
	{
		public int[] Statuses { get; set; }
		public int? RegionId { get; set; }
		public string CustomerId { get; set; }
		public string ManagerId { get; set; }
		public string RegionManagerId { get; set; }
		public int[] Organizations { get; set; }

	}
}