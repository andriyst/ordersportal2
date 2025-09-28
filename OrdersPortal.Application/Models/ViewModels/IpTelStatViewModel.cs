using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
    public class IpTelStatViewModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата з")]
        public DateTime DateStart { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата по")]
        public DateTime DateEnd { get; set; }

        public string Disposition { get; set; }

        public string Tel { get; set; }

    }
}