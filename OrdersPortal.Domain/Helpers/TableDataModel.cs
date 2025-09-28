
namespace OrdersPortal.Domain.Helpers
{
	public class TableDataModel
	{
		public string Order { get; set; }
		public string Sort { get; set; }
		public string Search { get; set; }
		public int Offset { get; set; }
		public int Limit { get; set; }
	}
}