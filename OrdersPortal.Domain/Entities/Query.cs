using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
	public class Query
	{
		public int QueryId { get; set; }

		[Display(Name = "Назва Запиту")]
		public string QueryName { get; set; }

		[Display(Name = "Опис Запиту")]
		public string QueryDesc { get; set; }

		[Display(Name = "Текст Запиту")]
		public string QueryText { get; set; }

	}
}