using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class HelpServiceLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string OrderPortalUserId { get; set; }
        public virtual OrderPortalUser OrderPortalUser { get; set; }
        public int HelpServiceContactId { get; set; }
        public virtual HelpServiceContact HelpServiceContact { get; set; }
        public bool Success { get; set; }
        public HelpServiceCallsEnum HelpServiceCall { get; set; }
    }

    public enum HelpServiceCallsEnum
    {
        Phone = 0,
        Email = 1,
        Telegram = 2,
        Sms =3
    } 
}