using System;

namespace OrdersPortal.Domain.Entities
{
	public class UploadFile
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string Description{ get; set; }
		public DateTime CreateDate { get; set; }
		public string AuthorId { get; set; }
		public OrderPortalUser Author { get; set; }
	}
}
