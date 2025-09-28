using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersPortal.Domain.Entities
{
	public class Db1SOrderPartsNumbers
	{
		[Key]
		public int Db1SOrderPartsNumberId { get; set; }

	
		public int OrderPartsId { get; set; }

		public virtual OrderParts OrderParts { get; set; }

		[Display(Name = "Номер в 1С")]
		public string Db1SOrderPartsNumber { get; set; }

		[Display(Name = "Статус в 1С")]
		public string Db1SOrderPartsStatus { get; set; }


		[Display(Name = "Підтвердження")]
		public bool Db1SOrderPartsConfirm { get; set; }


		[Display(Name = "Ознака оплати в 1С")]
		public bool Db1SOrderPartsPayed { get; set; }

		[Display(Name = "Дата Оплати в 1С")]
		public DateTime? Db1SOrderPartsDatePayed { get; set; }

		[Display(Name = "Дата Створення Замовлення в 1С")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Db1SOrderPartsDateCreate { get; set; }

        public byte[] OrderImage { get; set; }
        

	}
}