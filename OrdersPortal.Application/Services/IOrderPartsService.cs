using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Services
{
	public interface IOrderPartsService
	{
		AddOrderPartsViewModel PrepareAddViewModel();
		void AddOrderParts(AddOrderPartsViewModel viewModel);
		string Get1COrderPartsNumberByOrderId(int orderPartsId);
		Db1SOrderPartsNumbers Get1COrderPartsByOrderPartsId(int orderPartsId);
		byte[] Get1COrderPartsNumberFileByOrderPartsId(string db1cOrderPartsNumber);
		void Confirm1SOrderParts(int db1SOrderPartsNumberId);

	}
}
