using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersPortal.Domain.Entities
{
    public class Complaint
    {

        public Complaint()
        {
            this.ComplaintsMessage = new HashSet<ComplaintsMessage>();
            this.ComplaintPhoto = new HashSet<ComplaintPhoto>();
            this.ComplaintDecisions = new HashSet<ComplaintDecision>();
            this.ComplaintOrderSeries = new HashSet<ComplaintOrderSerie>();

        }
        [Key]
        public int ComplaintId { get; set; }

        //Дата завантаження рекламації
        public DateTime ComplaintDate { get; set; }

        //Номер замовлення яке в рекламації
        public string ComplaintOrderNumber { get; set; }

        //Номер акту на  рекламацію
        public string ComplaintActNumber { get; set; }

        //Дата акту на  рекламацію
        public DateTime? ComplaintActDate { get; set; }

        //Опис рекламації дилером
        public string ComplaintDescription { get; set; }

        // Дата доставки замовлення 
        public DateTime ComplaintOrderDeliverDate { get; set; }

        //Дата визначення рекламації
        public DateTime ComplaintOrderDefineDate { get; set; }

        //Тип предмету рекламіції (Вікно/Двері/Нестандарт/Склопакет/Зєднувач/Комплектуючі

        //ПРичина рекламації (витягується з 1С)
        public string ComplaintObject { get; set; }


        // Варіанти вирішення рекламації
        //public string ComplaintSolution { get; set; }



        // Орієнтовна дата усунення рекламації ???????????????????
        public DateTime ComplaintCompleteDate { get; set; }

        // Контрагент який подав рекламацію
        public string CustomerId { get; set; }
        public OrderPortalUser Customer { get; set; }

        // Відповідальний сервісний менеджер
        public string ManagerId { get; set; }
        public OrderPortalUser Manager { get; set; }

        // Результат Рекламації
        public string ComplaintResult { get; set; }



        //Статус Рекламації

        public int StatusId { get; set; }
        public Status Status { get; set; }



        //  Замовленя в 1С для цієї рекламації

        public string Complaint1cOrder { get; set; }

        //Дата запуску в роботу (переробку) рекламації
        public DateTime ComplaintWorkStartDate { get; set; }

        //Ознака Термінової рекламації
        public Boolean ComplaintUrgent { get; set; }

        // Повідомлення (Чат) в рекламації
        public virtual ICollection<ComplaintsMessage> ComplaintsMessage { get; set; }

        //Фото рекламації

        public virtual ICollection<ComplaintPhoto> ComplaintPhoto { get; set; }
        public virtual ICollection<ComplaintOrderSerie> ComplaintOrderSeries { get; set; }


        // Вирішення спорів (причина/вирішення) в рекламації
        public virtual ICollection<ComplaintDecision> ComplaintDecisions { get; set; }

    }
}