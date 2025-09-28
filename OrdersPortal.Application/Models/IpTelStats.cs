using System;

namespace OrdersPortal.Application.Models
{
    public class IpTelStats
    {
        public string UserName { get; set;}
        public string TelNumber { get; set; }
        public DateTime CallDateTime { get; set; }
        public string FromNumber { get; set; }
        public string ToNumber { get; set; }
        public string Disposition { get; set; }
        public string Duration { get; set; }

    }

    public class IpTelStatSummary
    {
        public string UserName { get; set; }
        public string TelNumber { get; set; }
        public int AnsweredCalls { get; set; }
        public int NoAnsweredCalls { get; set; }
        public int DialedCalls { get; set; }

        public int TotalСalls ()
        {
            return AnsweredCalls + NoAnsweredCalls + DialedCalls;
        }
    }
}