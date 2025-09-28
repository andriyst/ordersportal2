using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Infrastructure.EntityFramework;

namespace OrdersPortal.Application.Services
{
	public class MessageService : IMessageService
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IOrderRepository _orderRepository;
		
		private readonly UserManager<OrderPortalUser> _userManager;

		public MessageService(IMessageRepository messageRepository,
			IOrderRepository orderRepository)
		{
			_messageRepository = messageRepository;
			_orderRepository = orderRepository;
			var dbContextProvider = DependencyResolver.Current.GetService<IDbContextProvider>();

			_userManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(dbContextProvider.DbContext));
		}
		public void AddOrderMessage(OrderMessageViewModel viewModel)
		{
			OrdersMessage orderMessage = new OrdersMessage
			{
				Message = viewModel.Message,
				MessageTime = DateTime.Now,
				MessageWriterId = viewModel.MessageWriterId,
				OrderId = viewModel.OrderId
			};
			_messageRepository.AddPermanent(orderMessage);

			var order = _orderRepository.GetById(viewModel.OrderId);
			order.LastMessageTime = orderMessage.MessageTime;

			_orderRepository.SaveChanges();

		}

		public List<OrderMessageViewModel> GetOrderMessages(int orderId)
		{
			List<OrderMessageViewModel> messages = _messageRepository.GetOrderMessages(orderId).ToList().Select(y => new OrderMessageViewModel
			{
				Message = y.Message,
				OrderId = y.OrderId,
				MessageTime = y.MessageTime,
				MessageWriterName = y.MessageWriter.FullName,
				MessageWriterRole = _userManager.GetRoles(y.MessageWriter.Id)?.FirstOrDefault()
			}).ToList();


			return messages;
		}
	}
}
