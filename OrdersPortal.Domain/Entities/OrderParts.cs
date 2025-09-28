using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Domain.Entities
{
	public class OrderParts
	{

		[Key]
		public int OrderPartsId { get; set; }

		//Дата додавання дозамовлення
		public DateTime OrderPartsDate { get; set; }

		//Номер замовлення до якого буде дозамовлення
		public string OrderNumber { get; set; }

		//public Db1SOrderPartsNumbers Db1SOrderPartsNumber { get; set; }
		//public int? Db1SOrderPartsNumberId { get; set; }

		//Опис рекламації дилером
		public string OrderPartsDescription { get; set; }

		//Причина дозамовлення
		public virtual OrderPartsReason OrderPartsReason { get; set; }
		public int OrderPartsReasonId { get; set; }

		public DateTime? OrderPartsDepartureDate { get; set; }
		public DateTime? OrderPartsDeliveryDate { get; set; }

		public virtual OrderPartsItem OrderPartsItem { get; set; }
		public int OrderPartsItemId { get; set; }



		public int StatusId { get; set; }
		public Status Status { get; set; }


		// Контрагент який подав рекламацію
		public string CustomerId { get; set; }
		public OrderPortalUser Customer { get; set; }

		// Відповідальний сервісний менеджер
		public string ManagerId { get; set; }
		public OrderPortalUser Manager { get; set; }
	}
}
