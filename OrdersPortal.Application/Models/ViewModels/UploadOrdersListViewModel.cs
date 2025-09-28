using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class UploadOrdersListViewModel
	{

		[Display(Name = "Id")]
		public int OrderId { get; set; }

		[Display(Name = "Номер")]
		public string OrderNumber { get; set; }

		[Display(Name = "Номер в 1с")]
        public List<string> Db1SOrderNumbers { get; set; }
		
		[Display(Name = "Дата Завантаження")]
		//public string OrderDateCreate { get; set; }
        public DateTime OrderDateCreate { get; set; }

		[Display(Name = "Дата Обробки")]
		//public string OrderDateProgress { get; set; }
        public DateTime? OrderDateProgress { get; set; }

		[Display(Name = "Дата Запуску в виробництво")]
		//public string OrderDateComplete { get; set; }
        public DateTime? OrderDateComplete { get; set; }

		[Display(Name = "Відповідальни менеджер")]
		public string ManagerName { get; set; }

		[Display(Name = "Дилер")]
		public string CustomerName { get; set; }

		[Display(Name = "Ім'я файлу")]
		public string File { get; set; }

		[Display(Name = "Статус")]
		public string StatusName { get; set; }

		[Display(Name = "Статус Id")]
		public int StatusId { get; set; }

		[Display(Name = "К-сть конструкцій")]
		public double OrderNumberContructions { get; set; }

		[Display(Name = "Останній Коментар")]
		public string LastMessage { get; set; }

        [Display(Name = "Автор Останнього Коментаря")]
        public string LastMessageWriter { get; set; }

        [Display(Name = "Час Останнього Коментаря")]
        public DateTime LastMessageTime { get; set; }

		

	}
}