using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Application.Services
{
	public class HelpServiceService : IHelpServiceService
	{
		private readonly IHelpServiceContactRepository _helpServiceContactRepository;
		private readonly IHelpServiceLogRepository _helpServiceLogRepository;
		private readonly IAccountRepository _accountRepository;
		private readonly ApplicationContext _applicationContext;
		

		public HelpServiceService(IHelpServiceContactRepository helpServiceContactRepository, IHelpServiceLogRepository helpServiceLogRepository,
			IAccountRepository accountRepository, ApplicationContext applicationContext)
		{
			_helpServiceContactRepository = helpServiceContactRepository;
			_helpServiceLogRepository = helpServiceLogRepository;
			_accountRepository = accountRepository;
			_applicationContext = applicationContext;
		}

		public void SaveHelpServiceContacts(HelpServiceConfigureViewModel model)
		{
			if (model != null)
			{
				List<HelpServiceContact> contacts = _helpServiceContactRepository.GetAll().ToList();

				var salesContact = contacts.FirstOrDefault(x => x.HelpServiceType == HelpServiceTypesEnum.Sales);
				if (salesContact != null)
				{
					salesContact.ContactName = model.SalesContactName;
					salesContact.Phone = model.SalesPhone;
					salesContact.Email = model.SalesEmail;
					salesContact.TelegramId = model.SalesTelegramId;
				}
				else
				{
					salesContact = new HelpServiceContact
					{
						ContactName = model.SalesContactName,
						Phone = model.SalesPhone,
						Email = model.SalesEmail,
						TelegramId = model.SalesTelegramId,
						HelpServiceType = HelpServiceTypesEnum.Sales
					};

					_helpServiceContactRepository.Add(salesContact);
				}


				var serviceContact = contacts.FirstOrDefault(x => x.HelpServiceType == HelpServiceTypesEnum.Service);
				if (serviceContact != null)
				{
					serviceContact.ContactName = model.ServiceContactName;
					serviceContact.Phone = model.ServicePhone;
					serviceContact.Email = model.ServiceEmail;
					serviceContact.TelegramId = model.ServiceTelegramId;
				}
				else
				{
					serviceContact = new HelpServiceContact
					{
						ContactName = model.ServiceContactName,
						Phone = model.ServicePhone,
						Email = model.ServiceEmail,
						TelegramId = model.ServiceTelegramId,
						HelpServiceType = HelpServiceTypesEnum.Service
					};

					_helpServiceContactRepository.Add(serviceContact);
				}
				var logisticContact = contacts.FirstOrDefault(x => x.HelpServiceType == HelpServiceTypesEnum.Logistic);

				if (logisticContact != null)
				{
					logisticContact.ContactName = model.LogisticContactName;
					logisticContact.Phone = model.LogisticPhone;
					logisticContact.Email = model.LogisticEmail;
					logisticContact.TelegramId = model.LogisticTelegramId;
				}
				else
				{
					logisticContact = new HelpServiceContact
					{
						ContactName = model.LogisticContactName,
						Phone = model.LogisticPhone,
						Email = model.LogisticEmail,
						TelegramId = model.LogisticTelegramId,
						HelpServiceType = HelpServiceTypesEnum.Logistic
					};

					_helpServiceContactRepository.Add(logisticContact);
				}

				_helpServiceContactRepository.SaveChanges();
			}
		}

		public HelpServiceConfigureViewModel GetHelpServiceContacts()
		{
			List<HelpServiceContact> model = _helpServiceContactRepository.GetAll().ToList();

			HelpServiceConfigureViewModel result = new HelpServiceConfigureViewModel();

			foreach (var contact in model)
			{
				if (contact.HelpServiceType == HelpServiceTypesEnum.Sales)
				{
					result.SalesContactName = contact.ContactName;
					result.SalesPhone = contact.Phone;
					result.SalesEmail = contact.Email;
					result.SalesTelegramId = contact.TelegramId;
				}
				if (contact.HelpServiceType == HelpServiceTypesEnum.Service)
				{
					result.ServiceContactName = contact.ContactName;
					result.ServicePhone = contact.Phone;
					result.ServiceEmail = contact.Email;
					result.ServiceTelegramId = contact.TelegramId;
				}
				if (contact.HelpServiceType == HelpServiceTypesEnum.Logistic)
				{
					result.LogisticContactName = contact.ContactName;
					result.LogisticPhone = contact.Phone;
					result.LogisticEmail = contact.Email;
					result.LogisticTelegramId = contact.TelegramId;
				}

			}

			return result;
		}

		public bool CallManager(HelpServiceTypesEnum helpServiceType)
		{

			var currentUser = _accountRepository.GetById(_applicationContext.AccountId);

			var lastCall = _helpServiceLogRepository.GetAll().Where(x => x.OrderPortalUserId == currentUser.Id).OrderByDescending(x => x.CreateDate).FirstOrDefault()?.CreateDate;

			if (lastCall != null && lastCall > DateTime.Now.AddMinutes(-5))
			{
				//to do - Show Message to offen call
				return false;
			}
			else
			{
				var helpServiceLog = new HelpServiceLog
				{
					OrderPortalUserId = currentUser.Id,
					CreateDate = DateTime.Now,
					HelpServiceCall = HelpServiceCallsEnum.Telegram,
					HelpServiceContactId = _helpServiceContactRepository.GetContactByType(helpServiceType)?.Id ?? 0,
					Success = false
				};

				var contactPerson = _helpServiceContactRepository.GetContactByType(helpServiceType);

				var destinationId = contactPerson.TelegramId;
				try
				{
					//string urlString = "http://192.168.200.196/bot9/target_message?username_1c={0}&text={1}";


					// string userId = destinationId; //"cure";
					string text = $" Терміново зателефонуйте клієнту {HttpUtility.UrlEncode(currentUser.FullName)} за номером телефону {HttpUtility.UrlEncode(currentUser.Tel)}";

					string text2 = $" Дилер {HttpUtility.UrlEncode(currentUser.FullName)} за номером телефону {HttpUtility.UrlEncode(currentUser.Tel)}, надіслав повідомлення про терміновий виклик для {contactPerson.ContactName}";

					string sos = "\U0001F198";



					SendTelegramMessage(destinationId, sos + text);
					//Копія для Манюк Іри
					SendTelegramMessage("Манюк_І", sos + text2);
					if (helpServiceType == HelpServiceTypesEnum.Logistic)
					{
						SendTelegramMessage("Яковійчук_О", sos + text2);
					}

					helpServiceLog.Success = true;
				}
				catch (Exception ex)
				{
					ex = ex;
				}
				_helpServiceLogRepository.Add(helpServiceLog);
				_helpServiceLogRepository.SaveChanges();
				return true;
			}
		}
	

		public HelpServiceStatsViewModel GetHelpServiceStats(string startDate, string endDate)
		{

			HelpServiceStatsViewModel model = new HelpServiceStatsViewModel();

			if (string.IsNullOrEmpty(startDate))
			{
				model.StartDate = _helpServiceLogRepository.GetAll().OrderBy(x => x.CreateDate).FirstOrDefault()?.CreateDate ?? DateTime.Today;
			}
			else
			{
				model.StartDate = Convert.ToDateTime(startDate);
			}
			if (string.IsNullOrEmpty(endDate))
			{
				model.EndDate = DateTime.Now;
			}
			else
			{
				model.EndDate = Convert.ToDateTime(endDate).AddDays(1).AddMinutes(-1);
			}

			model.HelpServiceLogs = _helpServiceLogRepository.GetByPeriodDesc(model.StartDate, model.EndDate);
			                                            
			return model;
		}


		private bool SendTelegramMessage(string telegramId, string message)
		{
			try
			{
				string urlString = "http://192.168.200.196/bot9/target_message?username_1c={0}&text={1}";


				string userId = telegramId;// "cure";Яковійчук_О


				urlString = String.Format(urlString, userId, message);

				WebRequest request = WebRequest.Create(urlString);

				Stream rs = request.GetResponse().GetResponseStream();

				StreamReader reader = new StreamReader(rs);

				string line = "";

				StringBuilder sb = new StringBuilder();
				while (line != null)
				{
					line = reader.ReadLine();
					if (line != null)
						sb.Append(line);
				}

				string response = sb.ToString();
				return true;
			}
			catch (Exception ex)
			{
				ex = ex;
				return false;
			}

		}
	}

}
