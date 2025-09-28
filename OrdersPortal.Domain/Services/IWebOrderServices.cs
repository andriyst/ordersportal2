using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrdersPortal.Domain.Dto;

namespace OrdersPortal.Domain.Services
{
	public interface IWebOrderServices
	{
		double GetCustomerAdvance(string customerContrCode);
		bool ProceedPayments(List<PaymentDocumentDataDto> paymentList);
	}
}
