using System.Collections.Generic;

namespace OrdersPortal.Domain.Helpers
{
	public class JsonDataTableModel<T>
	{
		public int total { get; set; }
		public IEnumerable<T> rows { get; set; }
	}
}