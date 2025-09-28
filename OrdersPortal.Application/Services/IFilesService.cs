using System.Collections.Generic;
using OrdersPortal.Application.Models.ViewModels;

namespace OrdersPortal.Application.Services
{
	public interface IFilesService
	{
		List<UploadFilesListViewModel> GetList();
		void UploadFile(UploadFileViewModel viewModel);
		void DeleteFile(int id);
	}
}
