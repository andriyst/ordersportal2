using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Domain.Repositories
{
	public interface IFilesRepository : IRepository<UploadFile>
	{
		List<UploadFile> GetList();
	}
}
