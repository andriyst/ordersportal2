using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class UploadFileViewModel
	{

		public string FileName { get; set; }
		[Required]
		[Display(Name = "Файл")]
		public HttpPostedFileBase UploadFile { get; set; }
		[Required]
		[Display(Name = "Опис файлу")]
		public string Description { get; set; }


	}
}
