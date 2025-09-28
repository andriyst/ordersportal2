using System;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrderMessageViewModel
	{
		//--------------------------
		public string MessageWriterName { get; set; }
		public string MessageWriterId{ get; set; }
		public string MessageWriterRole { get; set; }

		//---------------------------	
		public DateTime MessageTime { get; set; }

		//---------------------------	
		public string Message { get; set; }

		//---------------------------
		public int OrderId { get; set; }
	
	}
}