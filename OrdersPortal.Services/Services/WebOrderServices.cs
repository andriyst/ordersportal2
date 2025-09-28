using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Models;
using OrdersPortal.Domain.Repositories;
using OrdersPortal.Domain.Services;
using OrdersPortal.Services.WebOrder1cService;
using OrdersPortal.Services.WebPortal1cService;
using OrdersPortal.Services.WebPortalComplaints1cService;

namespace OrdersPortal.Services.Services
{
	public class WebOrderServices : IWebOrderServices
	{
		private readonly IAccountRepository _accountRepository;

		private readonly WebOrderPortTypeClient _webService;
		private readonly cportal_vsPortTypeClient _service;

		//private const string WebServiceEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/weborder";
		//private const string ServiceEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/cportal_vs";

		private const string WebServiceEndPointAddress = "http://192.168.1.10/oknastyle/ws/weborder";
		private const string ServiceEndPointAddress = "http://192.168.1.10/oknastyle/ws/cportal_vs";



		public WebOrderServices(IAccountRepository accountRepository)
		{

			_accountRepository = accountRepository;

			BasicHttpBinding myBinding = new BasicHttpBinding();

			myBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
			myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
			myBinding.MaxReceivedMessageSize = 2147483647;
			myBinding.MaxBufferSize = 2147483647;
			myBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);



			EndpointAddress ea = new EndpointAddress(@ServiceEndPointAddress);
			_service = new cportal_vsPortTypeClient(myBinding, ea);

			_service.ClientCredentials.UserName.UserName = "webuser";
			_service.ClientCredentials.UserName.Password = "webP@ssw0rd2014";


			EndpointAddress webService = new EndpointAddress(WebServiceEndPointAddress);
			_webService = new WebOrderPortTypeClient(myBinding, webService);

			_webService.ClientCredentials.UserName.UserName = "webuser";
			_webService.ClientCredentials.UserName.Password = "webP@ssw0rd2014";

		}

		public double GetCustomerAdvance(string customerContrCode)
		{
			var customerAdvanceContractSum = _webService.GetAdvanceContractSumm(CustomerHelper.GetContrAgentFullCode(customerContrCode));

			return customerAdvanceContractSum.Summ;
		}
		public bool ProceedPayments(List<PaymentDocumentDataDto> paymentList)
		{
			try
			{
				PaymentDocumentDataList list = new PaymentDocumentDataList();
				foreach (var item in paymentList)
				{
					list.Add(new PaymentDocumentData
					{
						pay_sum = item.PaymentSum,
						order_number = item.OrderNumber
					});
				}

				var result = _webService.CreatePaymentDocument(list);

				if (result == "Все ОК")
				{
					return true;
				}
				
			}
			catch (Exception ex)
			{
				ex = ex;
			}

			return false;
		}

	}
}