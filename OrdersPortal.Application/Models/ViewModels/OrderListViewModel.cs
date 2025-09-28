using System;
using System.Collections.Generic;
using System.Linq;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class OrderListViewModel
	{
		public int OrderId { get; set; }
		public string OrderNumber { get; set; }
		public List<string> Db1SOrderNumbers { get; set; }
		public string OrderDateCreate { get; set; }
		public string OrderDateProgress { get; set; }
		public string OrderDateComplete { get; set; }
		public string ManagerName { get; set; }
		public string ManagerId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerId { get; set; }
		public string File { get; set; }
		public string StatusName { get; set; }
		public int StatusId { get; set; }
		public string OrganizationName { get; set; }
		public double OrderNumberContructions { get; set; }
		public string LastMessage { get; set; }
		public string LastMessageWriter { get; set; }
		public string LastMessageTime { get; set; }

		public string AllOrderDate
		{
			get
			{
				var result = String.IsNullOrEmpty(OrderDateCreate) ? "<img src='/Images/sm_NO.png'>" : "<img src='/Images/sm_YES.png'> " + OrderDateCreate;
				result = result + "<br>" + (String.IsNullOrEmpty(OrderDateProgress) ? "<img src='/Images/sm_NO.png'>" : "<img src='/Images/sm_YES.png'> " + OrderDateProgress);
				result = result + "<br>" + (String.IsNullOrEmpty(OrderDateComplete) ? "<img src='/Images/sm_NO.png'>" : "<img src='/Images/sm_YES.png'> " + OrderDateComplete);

				return result;
			}
		}

		public static IQueryable<OrderListViewModel> ConvertFromEntities(IQueryable<Order> entities)
		{
			return entities.Select(ConvertFromEntity).AsQueryable();
		}

		public static OrderListViewModel ConvertFromEntity(Order entity)
		{

			OrderListViewModel result = new OrderListViewModel
			{
				ManagerId = entity.ManagerId,
				CustomerId = entity.CustomerId,
				OrderNumber = entity.OrderNumber,
				OrderNumberContructions = entity.OrderNumberContructions,
				OrderId = entity.OrderId,
				OrderDateCreate = entity.OrderDateCreate.ToString("dd.MM.yyyy HH:mm"),
				OrderDateProgress =  entity.OrderDateProgress.HasValue ? entity.OrderDateProgress.Value.ToString("dd.MM.yyyy HH:mm") : "",
				OrderDateComplete= entity.OrderDateComplete.HasValue ? entity.OrderDateComplete.Value.ToString("dd.MM.yyyy HH:mm") : "",
				StatusId = entity.StatusId,
				StatusName = entity.Status.StatusName,
				File = entity.File,
				OrganizationName = entity.Customer.OrderPortalUserOrganizations.FirstOrDefault().Organization.OrganizationName,
				//Db1SOrderNumbers = entity.Db1SOrderNumbers.Select(x => x.Db1SOrderNumber).ToList(),
				CustomerName = entity.Customer.FullName,
				ManagerName = entity.Manager?.FullName?? ""
			};
			
			return result;
		}
	}
}