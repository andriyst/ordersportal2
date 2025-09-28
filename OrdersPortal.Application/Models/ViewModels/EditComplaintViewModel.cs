using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrdersPortal.Domain.Dto;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models.ViewModels
{
    public class EditComplaintViewModel : IValidatableObject
	{
        public int ComplaintId { get; set; }
        [Required]
        [Display(Name = "Прізвище контрагента")]
        public string CustomerName { get; set; }


        [Display(Name = "Прізвище менеджера")]
        public string ManagerName { get; set; }

        //Дата завантаження рекламації
        [Display(Name = "Дата завантаження рекламації")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
        public DateTime ComplaintDate { get; set; }

        //Номер замовлення яке в рекламації
        [Display(Name = "Номер замовлення яке в рекламації")]
        public string ComplaintOrderNumber { get; set; }

        // Дата доставки замовлення 
        [Display(Name = "Дата доставки замовлення")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
        public DateTime ComplaintOrderDeliverDate { get; set; }

        //Дата визначення рекламації
        [Display(Name = "Дата визначення рекламації")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd'.'MM'.'yyyy}")]
        public DateTime ComplaintOrderDefineDate { get; set; }

        // Опис рекламації
        [Display(Name = "Опишіть рекламацію:")]
        public string ComplaintDescription { get; set; }

        // Варіанти вирішення рекламації
        [Display(Name = "Варіанти вирішення рекламації")]
        public List<SelectListItem> ComplaintSolutions { get; set; }
	    [Display(Name = "Варіант вирішення")]
	    [Required(ErrorMessage = "Поле 'Варіант вирішення рекламації' не вибране")]
		public int ComplaintSolution { get; set; }
        [Display(Name = "Варіанти причини рекламації")]
        public List<SelectListItem> ComplaintIssues { get; set; }
	    [Display(Name = "Варіант причини")]
	    [Required(ErrorMessage = "Поле 'Варіант причини рекламації' не вибране")]
		public int ComplaintIssue { get; set; }

        public ICollection<ComplaintDecision> ComplaintDecisions { get; set; }

        //-----------------------------------------------------

        public List<ComplaintOrderSerieDto> ComplaintsOrderSeries { get; set; }



        [Display(Name = "Додати Файл")]
        public IEnumerable<HttpPostedFileBase> File { get; set; }



        [Display(Name = "Коментар контрагента")]
        public string CommentMessage { get; set; }

        public List<ComplaintDetailPhotoViewModel> ComplaintPhotoThubmnails { get; set; }


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> errors = new List<ValidationResult>();
			
			if (this.ComplaintsOrderSeries != null && this.ComplaintsOrderSeries.Any())
			{
				if (this.ComplaintsOrderSeries.Count(x => x.Checked) == 0)
				{
					errors.Add(new ValidationResult("Не обрано жодної конструкції"));
				}
			}

			return errors;
		}
	}
}