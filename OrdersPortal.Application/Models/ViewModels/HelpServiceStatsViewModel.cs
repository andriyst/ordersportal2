using System;
using System.Collections.Generic;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Application.Models.ViewModels
{
    public class HelpServiceStatsViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<HelpServiceLog> HelpServiceLogs { get; set; }

    }
}