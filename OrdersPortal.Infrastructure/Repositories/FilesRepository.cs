using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{

	public class FilesRepository : Repository<UploadFile>, IFilesRepository
	{
		public FilesRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public List<UploadFile> GetList()
		{
			return DbSet.Include(x=>x.Author).ToList();
		}

		
	}
}
