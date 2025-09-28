using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrdersPortal.Domain.Entities
{
	public class Db1SOrderNumbers
	{
		[Key]
		public int Db1SOrderNumberId { get; set; }


		[Display(Name = "Номер в 1С")]
		public string Db1SOrderNumber { get; set; }

		[Display(Name = "Статус в 1С")]
		public string Db1SOrderStatus { get; set; }


		[Display(Name = "Підтвердження")]
		public bool Db1SOrderConfirm { get; set; }


		[Display(Name = "Ознака оплати в 1С")]
		public bool Db1SOrderPayed { get; set; }

		[Display(Name = "Дата Оплати в 1С")]
		public DateTime? Db1SOrderDatePayed { get; set; }

		[Display(Name = "Дата Створення Замовлення в 1С")]

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Db1SOrderDateCreate { get; set; }

		[Display(Name = "Кількість конструкцій в замовленні")]
		public double Db1SOrderNumberConstr { get; set; }

        public byte[] OrderImage { get; set; }
        
        public Nullable<int> OrderId { get; set; }

		public virtual Order Order { get; set; }
	}
    public class tmpDbOrder
    {
        public int tmpOrderId { get; set; }
       
    }
}