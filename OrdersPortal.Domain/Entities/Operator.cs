using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Operator
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public virtual OrderPortalUser OrderPortalUser { get; set; }
	}
}