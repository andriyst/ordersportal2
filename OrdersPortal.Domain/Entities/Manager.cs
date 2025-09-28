using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Manager
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public virtual OrderPortalUser OrderPortalUser { get; set; }
		public string Guid { get; set; }

	}

}