//using System;
//using System.Collections.Generic;
//using System.ServiceModel;

//using OrdersPortal.Domain.Dto;
//using OrdersPortal.Infrastructure.EntityFramework;
//using OrdersPortal.Services.Connected_Services.WebPortal1cService;

//namespace OrdersPortal.Services.Services
//{
//	public class VstWeb1cService
//	{
//		private OrderPortalDbContext _datacontext;

//		private readonly cportal_vsPortTypeClient _portalService;
//		//private const string ServiceEndPointAddress = "http://192.168.50.50/oknastyle_gm/ws/cportal_vs";
//		private const string ServiceEndPointAddress = "http://192.168.1.10/oknastyle/ws/cportal_vs";



//		public VstWeb1cService(OrderPortalDbContext datacontext)
//		{
//			_datacontext = datacontext;
//			BasicHttpBinding myBinding = new BasicHttpBinding();

//			myBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
//			myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
//			myBinding.MaxReceivedMessageSize = 2147483647;
//			myBinding.MaxBufferSize = 2147483647;
//			myBinding.ReceiveTimeout = new TimeSpan(0, 5, 0);

//			EndpointAddress ea = new EndpointAddress(@ServiceEndPointAddress);
//			_portalService = new cportal_vsPortTypeClient(myBinding, ea);

//			_portalService.ClientCredentials.UserName.UserName = "webuser";
//			_portalService.ClientCredentials.UserName.Password = "webP@ssw0rd2014";
//		}

//		public List<CustomerCodesListDto> GetCustomerCodesService()
//		{
//			List<CustomerCodesListDto> contrAgentsList = new List<CustomerCodesListDto>();
//			try
//			{
//				if (contrAgentsList.Count < 1)
//				{
//					var res = _portalService.get_kontr_list();

//					if (res != null)
//					{
//						foreach (var cn in res)
//						{
//							contrAgentsList.Add(new CustomerCodesListDto
//							{
//								CustomerName = cn.name.ToString(),
//								Code = Convert.ToInt32(cn.kod)
//							});
//						}
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				ex = ex;
//			}

//			return contrAgentsList;
//		}
//		public bool Set1CDeliveryStatus(string code, string orderNumber)
//		{
//			var check = false;
//			try
//			{
//				var res = _portalService.set_order_state_wait_for_pay(orderNumber, code);
//				check = res > 0;
//			}
//			catch (Exception ex)
//			{
//				ex = ex;
//			}

//			return check;
//		}
//	}
//}