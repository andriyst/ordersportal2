using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Customer
	{
		[Key]
		public int Id { get; set; }
		public string ManagerId { get; set; }
		public string RegionManagerId { get; set; }
		public int CustomerContrCode { get; set; }
		public bool? AutoConfirmOrder { get; set; }
		public bool? PermitFinanceInfo{ get; set; }
		[Required]
		public virtual OrderPortalUser OrderPortalUser { get; set; }

	}
}