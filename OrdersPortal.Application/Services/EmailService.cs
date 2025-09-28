//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net.Mail;
//using System.Net.Mime;
//using System.Text;
//using System.Web;

//using OrdersPortal.Application.Repositories;
//using OrdersPortal.Domain.Entities;
//using OrdersPortal.Infrastructure.EntityFramework;

//namespace OrdersPortal.Application.Services
//{
//	public class EmailService
//	{

//		private OrderPortalDbContext _dataOrderPortalDbContext;
//		private OrderPortalDBRepo _repo;
//		private UserRoleRepository _userRoleRepository;

//		public EmailService()
//		{
//			_dataOrderPortalDbContext = new OrderPortalDbContext();
//            _repo = new OrderPortalDBRepo(_dataOrderPortalDbContext);
//			_userRoleRepository = new UserRoleRepository(_dataOrderPortalDbContext);
//		}
//		public void RegisterEmailNotify(OrderPortalUser user, string password)
//		{
//			// Sending Email-----------------------------------------------
//			string body;
//			//Read template file from the App_Data folder
//			using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\App_Data\\EmailTemplates\\") + "AccountRegisterEmail.txt"))
//			{
//				body = sr.ReadToEnd();
//			}

//			string messageBody = string.Format(body, user.FullName, user.UserName, password);
			
//			//-------------------------------------------------

//			AlternateView alternateViewHtml = AlternateView.CreateAlternateViewFromString(messageBody, Encoding.UTF8, MediaTypeNames.Text.Html);
//			LinkedResource vstLogo = new LinkedResource(HttpContext.Current.Server.MapPath("/App_Data/EmailTemplates/logo.jpg"), MediaTypeNames.Image.Jpeg);

//			vstLogo.ContentId = "VSLogo";
			
//			alternateViewHtml.LinkedResources.Add(vstLogo);

			
//			//----------------------------------------
//			List<string> EmailList = new List<string>();

//			string RoleName = _userRoleRepository.GetRoleNameByUserId(user.Id);
//			if (RoleName == "customer")
//			{
//				EmailList.Add(user.Email);
//				OrderPortalUser manager = _repo.GetManagerByCustomerId(user.Id);
//				OrderPortalUser regionmanager = _userRoleRepository.GetRegionManagerByCustomerId(user.Id);
//				EmailList.Add(manager.Email);
//				EmailList.Add(regionmanager.Email);
//			}
//			 else
//			{
//				EmailList.Add(user.Email);				
//			}

//			string subject = "Реєстрація на Порталі Замовлень";
//			foreach (var a in EmailList)
//			{
//				EmailNotify(a, messageBody, subject);
//			}
//		}

//        public void ResetPasswordEmailNotify(OrderPortalUser user, string password)
//        {
//            // Sending Email-----------------------------------------------
//            string body;
//            //Read template file from the App_Data folder
//            using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\App_Data\\EmailTemplates\\") + "ChangePasswordEmail.txt"))
//            {
//                body = sr.ReadToEnd();
//            }

//            string messageBody = string.Format(body, user.FullName, user.UserName, password);

//            //-------------------------------------------------

//            AlternateView alternateViewHtml = AlternateView.CreateAlternateViewFromString(messageBody, Encoding.UTF8, MediaTypeNames.Text.Html);
//            LinkedResource vstLogo = new LinkedResource(HttpContext.Current.Server.MapPath("/App_Data/EmailTemplates/logo.jpg"), MediaTypeNames.Image.Jpeg);

//            vstLogo.ContentId = "VSLogo";

//            alternateViewHtml.LinkedResources.Add(vstLogo);


//            //----------------------------------------
//            List<string> EmailList = new List<string>();

//            //_user
//            var roleName = _userRoleRepository.GetRoleNameByUserId(user.Id);
//            //    Roles.GetRolesForUser(user.UserName).First();

            

          
//             //   user.Roles.Select(x => x.Role.Name).SingleOrDefault();
            
//            if (roleName == "customer")
//            {
//                EmailList.Add(user.Email);
//                OrderPortalUser manager = _repo.GetManagerByCustomerId(user.Id);
//                OrderPortalUser regionmanager = _userRoleRepository.GetRegionManagerByCustomerId(user.Id);
//                EmailList.Add(manager.Email);
//                EmailList.Add(regionmanager.Email);
//            }
//            else if (roleName == "manager")
//            {
//                EmailList.Add(user.Email);
//                OrderPortalUser regionmanager = _userRoleRepository.GetRegionManagerByCustomerId(user.Id);
//                EmailList.Add(regionmanager.Email);

//            }
//            else if (roleName == "regionmanager")
//            {
//                EmailList.Add(user.Email);

//            }
//            else
//            {
//                EmailList.Add(user.Email);
//            }

//            string subject = "Зміна пароля на Порталі Замовлень";
//            foreach (var a in EmailList)
//            {
//                EmailNotify(a, messageBody, subject);
//            }
//        }

//		public void UploadOrderEmailNotify(Order order)
//		{
//			// Sending Email-----------------------------------------------
//			string body;
//			//Read template file from the App_Data folder
//			using (var sr = new StreamReader(HttpContext.Current.Server.MapPath("\\App_Data\\EmailTemplates\\") + "UploadOrderNotifyEmail.txt"))
//			{
//				body = sr.ReadToEnd();
//			}

//			OrderPortalUser manager = _repo.GetOrdersPortalUserById(order.ManagerId);
//			OrderPortalUser user = _repo.GetOrdersPortalUserById(order.CustomerId);

//			string messageBody = string.Format(body, user.FullName, order.OrderNumber, order.OrderNumberContructions, order.OrderDateCreate);

//			//-------------------------------------------------

//			AlternateView alternateViewHtml = AlternateView.CreateAlternateViewFromString(messageBody, Encoding.UTF8, MediaTypeNames.Text.Html);
//			LinkedResource vstLogo = new LinkedResource(HttpContext.Current.Server.MapPath("/App_Data/EmailTemplates/logo.jpg"), MediaTypeNames.Image.Jpeg);

//			vstLogo.ContentId = "VSLogo";

//			alternateViewHtml.LinkedResources.Add(vstLogo);


//			//----------------------------------------
//			List<string> EmailList = new List<string>();

			
//				//EmailList.Add(user.EMail);
//				//OrderPortalUser manager = _repo.GetManagerByCustomerId(user.Id);
//				//OrderPortalUser regionmanager = _repo.GetRegionManagerByCustomerId(user.Id);
//				EmailList.Add(manager.Email);
//				//EmailList.Add(regionmanager.EMail);
		

//			string subject = "Клієнт "+ user.FullName +" завантажив нове замовлення!";
//			foreach (var a in EmailList)
//			{
//				EmailNotify(a, messageBody, subject);
//			}
//		}
//		public void EmailNotify(string email, string message, string subject)
//		{
//			MailAddress from = new MailAddress("portal@viknastyle.com.ua", "Портал Замовлень");
//			// кому отправляем
//			MailAddress to = new MailAddress(email);


//			// создаем объект сообщения
//			MailMessage m = new MailMessage(from, to);
//			m.Bcc.Add("portal@viknastyle.com.ua");
						
//			// тема письма
//			m.Subject = subject;
//			// текст письма - включаем в него ссылку
//			m.Body = message;
			
//			m.IsBodyHtml = true;

//			SmtpClient client = new SmtpClient("mx.viknastyle.com.ua", 25);

//			client.UseDefaultCredentials = false;
//			client.EnableSsl = false;

//			try
//			{
//				client.Send(m);
//			}

//			catch (Exception ex)
//			{
//			//	ModelState.AddModelError("", ex.Message);
//			}

//		}
//	}
//}