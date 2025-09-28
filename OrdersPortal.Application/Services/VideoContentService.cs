using System.Collections.Generic;
using System.Linq;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Application.Services
{
	public class VideoContentService : IVideoContentService
	{
		private readonly IVideoContentRepository _videoContentRepository;

		public VideoContentService(IVideoContentRepository videoContentRepository)
		{
			_videoContentRepository = videoContentRepository;
		}

		public List<VideoContent> GetList()
		{
			return _videoContentRepository.GetAll().ToList();
		}

		public void Add(VideoContent model)
		{
			var url = model.Url.Substring(model.Url.LastIndexOf('/') + 1);
			model.Url = url;
			_videoContentRepository.AddPermanent(model);
		}

		public void Remove(int regionId)
		{
			var videoContent = _videoContentRepository.GetById(regionId);
			if (videoContent != null)
			{
				_videoContentRepository.Remove(videoContent);
				_videoContentRepository.SaveChanges();
			}
		}

		public void Edit(VideoContent model)
		{
			var result = _videoContentRepository.GetById(model.Id);
			result.Url = model.Url;
			result.Description = model.Description;
			_videoContentRepository.UpdatePermanent(result);
		}
	}
}
