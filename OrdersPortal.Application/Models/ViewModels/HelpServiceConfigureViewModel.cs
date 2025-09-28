using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
    public class HelpServiceConfigureViewModel
    {
        [Display(Name = "Ім'я контакту")]
        public string SalesContactName { get; set; }
        [Display(Name = "Телефон контакту")]
        public string SalesPhone { get; set; }
        [Display(Name = "E-mail контакту")]
        public string SalesEmail { get; set; }
        [Display(Name = "TelegramId контакту")]
        public string SalesTelegramId { get; set; }


        [Display(Name = "Ім'я контакту")]
        public string ServiceContactName { get; set; }
        [Display(Name = "Телефон контакту")]
        public string ServicePhone { get; set; }
        [Display(Name = "E-mail контакту")]
        public string ServiceEmail { get; set; }
        [Display(Name = "TelegramId контакту")]
        public string ServiceTelegramId { get; set; }


        [Display(Name = "Ім'я контакту")]
        public string LogisticContactName { get; set; }
        [Display(Name = "Телефон контакту")]
        public string LogisticPhone { get; set; }
        [Display(Name = "E-mail контакту")]
        public string LogisticEmail { get; set; }
        [Display(Name = "TelegramId контакту")]
        public string LogisticTelegramId { get; set; }

    }
}