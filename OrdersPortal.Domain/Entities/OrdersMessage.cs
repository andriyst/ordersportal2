using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersPortal.Domain.Entities
{
	public class OrdersMessage
	{
		[Key]
		public int OrdersMessageId { get; set; }

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
		public int OrderId { get; set; }
		public virtual Order Order { get; set; }


	}
}