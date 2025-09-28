using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class OrdersPortalLogging
    {
        [Key]
        public int LogId { get; set; }
        public DateTime LogDateTime { get; set; }
        public OrderPortalUser OrderPortalUser { get; set; }
        public string LogIpAddress { get; set; }
        public string LogAction { get; set; }

    }
}