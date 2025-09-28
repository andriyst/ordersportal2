using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

using NLog;

using OrdersPortal.Application.Context;
using OrdersPortal.Application.Models.ViewModels;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

using NLog;

namespace OrdersPortal.Application.Services
{
	public class FilesService : IFilesService
	{
		private readonly IAccountService _accountService;
		private readonly IFilesRepository _filesRepository;
		private readonly ApplicationContext _applicationContext;
		private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();


		public FilesService(IAccountService accountService, IFilesRepository filesRepository, ApplicationContext applicationContext)
		{
			_accountService = accountService;
			_filesRepository = filesRepository;
			_applicationContext = applicationContext;
		}

		public List<UploadFilesListViewModel> GetList()
		{
			
			List<UploadFilesListViewModel> viewModel = new List<UploadFilesListViewModel>();
			try
			{
				var files = _filesRepository.GetList();
				foreach (var file in files)
				{
					viewModel.Add(new UploadFilesListViewModel
					{
						Id = file.Id,
						FileName = file.FileName,
						FilePath = file.FilePath,
						Description = file.Description,
						Author = file.Author.FullName,
						CreateDate = file.CreateDate
					});
				}
			}
			catch (Exception ex)
			{
				_logger.Debug(ex);
			}
			return viewModel;

		}

		public void UploadFile(UploadFileViewModel viewModel)
		{
			try
			{
				if (viewModel != null)
				{
					OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);


					string filesPath = "/Files/Downloads/";
					var realFile = viewModel.UploadFile;

					var file = Path.GetFileName(realFile.FileName);

					string fullPath = filesPath;


					if (!Directory.Exists(HttpContext.Current.Server.MapPath(fullPath)))
					{
						Directory.CreateDirectory(HttpContext.Current.Server.MapPath(fullPath));
					}

					string fileNameOnly = Path.GetFileNameWithoutExtension(file);
					string extension = Path.GetExtension(file);

					string path = Path.Combine(HttpContext.Current.Server.MapPath(fullPath), fileNameOnly + extension);

					int count = 1;

					while (File.Exists(path))
					{
						string tempFileName = $"{fileNameOnly}({count++})";
						path = Path.Combine(HttpContext.Current.Server.MapPath(fullPath), tempFileName + extension);
					}

					string correctFilename = HttpContext.Current.Server.UrlPathEncode(Path.GetFileName(path));
					HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + correctFilename + "\"");

					viewModel.UploadFile.SaveAs(path);


					var entity = new UploadFile
					{
						Description = viewModel.Description,
						AuthorId = currentUser.Id,
						CreateDate = DateTime.Now,
						FileName = file,
						FilePath = filesPath
					};

					_filesRepository.Add(entity);
					_filesRepository.SaveChanges();

				}
			}
			catch (Exception ex)
			{
				_logger.Debug(ex);
			}
		}

		public void DeleteFile(int id)
		{
			try
			{
				if (id != null)
				{
					OrderPortalUser currentUser = _accountService.GetById(_applicationContext.AccountId);

					var entityFile  = _filesRepository.GetById(id);

					string fullPath = entityFile.FilePath + entityFile.FileName ;
					
					string path = Path.Combine(HttpContext.Current.Server.MapPath(fullPath));

					if (File.Exists(path))
					{
						// If file found, delete it    
						File.Delete(path);
						
					}
					else
					{
						_logger.Debug("File Not Found!");
					}

					_filesRepository.Remove(entityFile);
					_filesRepository.SaveChanges();

				}
			}
			catch (Exception ex)
			{
				_logger.Debug(ex);
			}
		}
	}
}
