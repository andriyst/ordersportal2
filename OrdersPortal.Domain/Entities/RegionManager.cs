using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class RegionManager
	{
		[Key]
		public int Id { get; set; }
		public string Guid { get; set; }
		[Required]
		public virtual OrderPortalUser OrderPortalUser { get; set; }
	}
}