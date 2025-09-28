using System;
using System.Collections.Generic;
using System.Linq;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrderPartsListViewModel
	{
		public int OrderPartsId { get; set; }
		public string OrderPartsNumber { get; set; }
		public string Db1SOrderPartsNumbers { get; set; }
		public string OrderPartsDate { get; set; }
		public string OrderPartsDepartureDate { get; set; }
		public string OrderPartsDeliveryDate { get; set; }

		public string OrderPartsItems { get; set; }
		public string OrderPartsReason { get; set; }
		public string OrderPartsDescription { get; set; }

		public string ManagerName { get; set; }
		public string ManagerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerId { get; set; }

		public string StatusName { get; set; }
		public int StatusId { get; set; }




		public static IList<OrderPartsListViewModel> ConvertFromEntities(IList<OrderParts> entities)
		{
			return entities.Select(ConvertFromEntity).ToList();
		}

		public static OrderPartsListViewModel ConvertFromEntity(OrderParts entity)
		{

			OrderPartsListViewModel result = new OrderPartsListViewModel
			{
				ManagerId = entity.ManagerId,
				CustomerId = entity.CustomerId,
				//OrderPartsNumber = entity.Db1SOrderPartsNumber?.Db1SOrderPartsNumber,

				//OrderPartsDescription = entity.OrderPartsDescription,

				OrderPartsDescription = (entity.OrderPartsDepartureDate != null && entity.OrderPartsReasonId == 2) ?
					entity.OrderPartsDescription + " <div style='color: orange'><b>Дата повернення на виробництво " + entity.OrderPartsDepartureDate.Value.ToShortDateString() + "р.</b></div>" :
					entity.OrderPartsDescription,


				OrderPartsItems = entity.OrderPartsItem.OrderPartsItemName,
				OrderPartsReason = entity.OrderPartsReason.OrderPartsReasonName,

				OrderPartsId = entity.OrderPartsId,
				OrderPartsDate = entity.OrderPartsDate.ToString("dd.MM.yyyy HH:mm"),
				OrderPartsDepartureDate = entity.OrderPartsDepartureDate?.ToString("dd.MM.yyyy HH:mm"),
				OrderPartsDeliveryDate = entity.OrderPartsDeliveryDate?.ToString("dd.MM.yyyy"),

				StatusId = entity.StatusId,
				StatusName = entity.Status.StatusName,

				//Db1SOrderNumbers = entity.Db1SOrderNumbers.Select(x => x.Db1SOrderNumber).ToList(),
				CustomerName = entity.Customer.FullName,
				ManagerName = entity.Manager?.FullName ?? ""
			};

			return result;
		}
	}
}