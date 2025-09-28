using System;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Application.Models.ViewModels
{
    public class UploadComplaintsListViewModel
    {
        public int ComplaintId { get; set; }

        //Дата завантаження рекламації
        public DateTime ComplaintDate { get; set; }

     
        [Display(Name = "Дата доставки замовлення")]

        public DateTime ComplaintOrderDeliverDate { get; set; }

        //
        [Display(Name = "Дата виявлення рекламації")]

        public DateTime ComplaintOrderDefineDate { get; set; }

     
        //Номер замовлення яке в рекламації
        public string ComplaintOrderNumber { get; set; }

       

     

        [Display(Name = "Відповідальни менеджер")]
        public string ManagerName { get; set; }

        [Display(Name = "Дилер")]
        public string CustomerName { get; set; }


        // Опис Рекламації
        [Display(Name = "Опис рекламації")]
        public string ComplaintDescription { get; set; }

        // Акт Рекламації
        [Display(Name = "Номер Акту рекламації")]
        public string ComplaintActNumber { get; set; }


        // Результат Рекламації
        [Display(Name = "Результат вирішення рекламації")]
        public string ComplaintIssue { get; set; }

        [Display(Name = "Варіанти дилера вирішення рекламації")]
        public string ComplaintSolution { get; set; }



        [Display(Name = "Номер створеної рекламації в 1с")]
        public string Complaint1cOrder { get; set; }
      


        //Статус Рекламації
        [Display(Name = "Статус готовності рекламації")]
        public string StatusName { get; set; }

        // ----------------------------- Чат ---------------------

        [Display(Name = "Останній Коментар")]
        public string LastMessage { get; set; }

        [Display(Name = "Автор Останнього Коментаря")]
        public string LastMessageWriter { get; set; }

        [Display(Name = "Час Останнього Коментаря")]
        public DateTime LastMessageTime { get; set; }
        public DateTime? ApproveDate { get; set; }

    }
}