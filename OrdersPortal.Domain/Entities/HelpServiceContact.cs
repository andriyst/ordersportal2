using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class HelpServiceContact
    {
        [Key]
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TelegramId { get; set; }
        public HelpServiceTypesEnum HelpServiceType { get; set; }
    }

    public enum HelpServiceTypesEnum
    {
        Sales = 0,
        Service = 1,
        Logistic = 2
    }
}