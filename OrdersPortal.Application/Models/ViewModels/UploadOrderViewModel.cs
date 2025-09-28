using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OrdersPortal.Infrastructure.EntityFramework;

namespace OrdersPortal.Application.Models.ViewModels
{
	public class UploadOrderViewModel : IValidatableObject
	{
						
		[Required]
		[Display(Name = "Прізвище контрагента")]
		public string CustomerName { get; set; }

		
		[Display(Name = "Прізвище менеджера")]
		public string ManagerName { get; set; }


		[Required]
		[Display(Name = "Номер замовл.")]
        [RegularExpression("([а-яА-ЯіІїЇєЄa-zA-Z0-9 .&'-_№#]{0,11}$)", ErrorMessage = "Некоректні символи або більше 11 символів")]
		public string OrderNumber { get; set; }

		[Required]
		[Display(Name = "Файл")]
		public HttpPostedFileBase File { get; set; }

		[Required]
		[Display(Name = "К-сть конструкцій")]
		public double OrderNumberContructions { get; set; }
		
		
		[Display(Name = "Коментар контрагента")]
		public string CommentMessage { get; set; }
		
		
	


		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> errors = new List<ValidationResult>();

			OrderPortalDbContext dbContext = new OrderPortalDbContext();

			string customerId = dbContext.Users.Where(x => x.UserName == CustomerName).Select(x => x.Id).SingleOrDefault();

			string[] ordersNumbers = dbContext.Orders.Where(x => x.CustomerId == customerId).Select(x => x.OrderNumber).ToArray();

			if (OrderNumber != null)
			{
				string strval = OrderNumber.Trim();
				for (int i = 0; i < ordersNumbers.Length; i++)
				{
					if (strval == ordersNumbers[i].Trim())
						errors.Add(new ValidationResult("Не унікальний номер замовлення"));
				}
			}

			if (OrderNumberContructions < 1)
			{
				errors.Add(new ValidationResult("Не вказано кількість конструкцій"));
			}	

			return errors;
		}
	}
}