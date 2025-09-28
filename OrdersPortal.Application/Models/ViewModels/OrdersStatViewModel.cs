namespace OrdersPortal.Application.Models.ViewModels
{
    public class OrdersStatViewModel
    {

        public int NumOrders { get; set; }
        public double NumOrdersContrustions { get; set; }        
        public int NumBestCustomerOrders{ get; set; }
        public string BestCustomerName { get; set; }
        public string ManagerName { get; set; }

    }
}