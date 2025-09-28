using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class UploadFilesListViewModel
	{
		public int Id { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
		public string Description { get; set; }
		public string Author { get; set; }
		public DateTime CreateDate { get; set; }

	}
}
