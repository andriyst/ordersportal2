using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


using OrdersPortal.Domain.Entities;
using OrdersPortal.Infrastructure.EntityFramework;

namespace OrdersPortal.Application.Services
{
    public class ShowOrdersSerivice
    {
        private OrderPortalDbContext _datacontext;
          private UserManager<OrderPortalUser> _userManager;
      
        
        public ShowOrdersSerivice(OrderPortalDbContext datacontext)
        {
            _datacontext = datacontext;
            _userManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(_datacontext));
           
        }

        //public List<UploadOrdersListViewModel> GetCustomerOrders(string customerId, List<int> statuses)
        //{
        //    List<UploadOrdersListViewModel> orderlist = new List<UploadOrdersListViewModel>();

        //    List<int> AllStatuses = _datacontext.Statuses.Select(x=>x.StatusId).ToList();

        

        //    List<int> StatusId = new List<int>();
        //    if (statuses != null)
        //    {
        //        StatusId.AddRange(statuses);
        //    }
        //    else
        //    {
        //        StatusId.AddRange(AllStatuses);

        //    }

        //    OrdersMessage EmptyLastMessage = new OrdersMessage()
        //    {
        //        Message = "Немає Повідомлень"
        //    };


        //     orderlist = _datacontext.Orders
        //            .Where(x => x.CustomerId == customerId)                                                   
        //            .Where(x => StatusId.Contains(x.StatusId))
        //           .Join(
        //         _datacontext.OrdersMessages
        //         .Join(
        //         _datacontext.OrdersMessages.GroupBy(c => c.OrderId)
        //          .Select(v => new
        //          {
        //              OrderId = v.Key,
        //              MessageTime = v.Max(z => z.MessageTime)

        //          }),
        //          q => q.MessageTime,
        //          q2 => q2.MessageTime,
        //          (q, q2) => new
        //          {
        //              q.MessageTime,
        //              q.Message,
        //              q.MessageWriter.FullName,
        //              q.OrderId
        //          }
        //          ),
        //          e => e.OrderId,
        //          o => o.OrderId,
        //          (e, o) => new UploadOrdersListViewModel()
        //          {
        //              OrderId = e.OrderId,

        //              CustomerName = e.Customer.FullName,

        //              ManagerName = e.Manager.FullName,
        //              File = e.File,
        //              Db1SOrderNumbers = e.Db1SOrderNumbers.Select(x => x.Db1SOrderNumber).ToList(),

        //              OrderDateComplete = e.OrderDateComplete,
        //              OrderDateCreate = e.OrderDateCreate,
        //              OrderDateProgress = e.OrderDateProgress,
        //              OrderNumber = e.OrderNumber,
        //              OrderNumberContructions = e.OrderNumberContructions,
        //              StatusId = e.StatusId,
        //              StatusName = e.Status.StatusName,
        //              LastMessage = o.Message,
        //              LastMessageTime = o.MessageTime,
        //              LastMessageWriter = o.FullName
        //          }
        //         )
        //         .OrderByDescending(d => d.LastMessageTime)
        //         .ToList();          
              
        //        return orderlist;                
        //}
        //public List<UploadOrdersListViewModel> GetManagerOrders(string managerId, List<int> statuses)
        //{
        //    List<UploadOrdersListViewModel> orderlist = new List<UploadOrdersListViewModel>();

        //    List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();
        //    var regionManager = _repo.GetRegionManagerByManagerId(managerId);
        //    var managers = _repo.GetManagersByRegionManagerId(regionManager.Id).Select(x => x.Id).ToList();

        //    List<int> StatusId = new List<int>();
        //    if (statuses != null)
        //    {
        //        StatusId.AddRange(statuses);
        //    }
        //    else
        //    {
        //        StatusId.AddRange(AllStatuses);

        //    }

        //    OrdersMessage EmptyLastMessage = new OrdersMessage()
        //    {
        //        Message = "Немає Повідомлень"
        //    };


        //    orderlist = _datacontext.Orders
        //          .Where(x => managers.Contains(x.ManagerId))
        //          .Where(x => StatusId.Contains(x.StatusId))
        //        .Join(
        //         _datacontext.OrdersMessages
        //         .Join(
        //         _datacontext.OrdersMessages.GroupBy(c => c.OrderId)
        //          .Select(v => new
        //          {
        //              OrderId = v.Key,
        //              MessageTime = v.Max(z => z.MessageTime)

        //          }),
        //          q => q.MessageTime,
        //          q2 => q2.MessageTime,
        //          (q, q2) => new
        //          {
        //              q.MessageTime,
        //              q.Message,
        //              q.MessageWriter.FullName,
        //              q.OrderId
        //          }
        //          ),
        //          e => e.OrderId,
        //          o => o.OrderId,
        //          (e, o) => new UploadOrdersListViewModel
        //          {
        //              OrderId = e.OrderId,

        //              CustomerName = e.Customer.FullName,

        //              ManagerName = e.Manager.FullName,
        //              File = e.File,
        //              Db1SOrderNumbers = e.Db1SOrderNumbers.Select(x => x.Db1SOrderNumber).ToList(),

        //              OrderDateComplete = e.OrderDateComplete,
        //              OrderDateCreate = e.OrderDateCreate,
        //              OrderDateProgress = e.OrderDateProgress,
        //              OrderNumber = e.OrderNumber,
        //              OrderNumberContructions = e.OrderNumberContructions,
        //              StatusId = e.StatusId,
        //              StatusName = e.Status.StatusName,
        //              LastMessage = o.Message,
        //              LastMessageTime = o.MessageTime,
        //              LastMessageWriter = o.FullName
        //          }
        //         )
        //         .OrderByDescending(d => d.LastMessageTime)
        //         .ToList();


        //    return orderlist;

        //}
        //public List<UploadOrdersListViewModel> GetRegionManagerOrders(string regionmanagerId, List<int> statuses)
        //{
        //    List<UploadOrdersListViewModel> orderlist = new List<UploadOrdersListViewModel>();
        //    var man = _repo.GetManagersByRegionManagerId(regionmanagerId).Select(x => x.Id);
        //    var regionmanager = _repo.GetRegionManagerById(regionmanagerId);
        //    var regionId = regionmanager.RegionId;
        //    List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();

        //    List<int> StatusId = new List<int>();
        //    if (statuses != null)
        //    {
        //        StatusId.AddRange(statuses);
        //    }
        //    else
        //    {
        //        StatusId.AddRange(AllStatuses);

        //    }

        //    OrdersMessage EmptyLastMessage = new OrdersMessage
        //    {
        //        Message = "Немає Повідомлень"
        //    };


        //    orderlist = _datacontext.Orders
        //          //.Where(x => man.Contains(x.ManagerId))
        //          .Where(x=>x.Customer.RegionId == regionId)
        //          .Where(x => StatusId.Contains(x.StatusId))
        //        .Join(
        //         _datacontext.OrdersMessages
        //         .Join(
        //         _datacontext.OrdersMessages.GroupBy(c => c.OrderId)
        //          .Select(v => new
        //          {
        //              OrderId = v.Key,
        //              MessageTime = v.Max(z => z.MessageTime)

        //          }),
        //          q => q.MessageTime,
        //          q2 => q2.MessageTime,
        //          (q, q2) => new
        //          {
        //              q.MessageTime,
        //              q.Message,
        //              q.MessageWriter.FullName,
        //              q.OrderId
        //          }
        //          ),
        //          e => e.OrderId,
        //          o => o.OrderId,
        //          (e, o) => new UploadOrdersListViewModel
        //          {
        //              OrderId = e.OrderId,

        //              CustomerName = e.Customer.FullName,

        //              ManagerName = e.Manager.FullName,
        //              File = e.File,
        //              Db1SOrderNumbers = e.Db1SOrderNumbers.Select(x => x.Db1SOrderNumber).ToList(),

        //              OrderDateComplete = e.OrderDateComplete,
        //              OrderDateCreate = e.OrderDateCreate,
        //              OrderDateProgress = e.OrderDateProgress,
        //              OrderNumber = e.OrderNumber,
        //              OrderNumberContructions = e.OrderNumberContructions,
        //              StatusId = e.StatusId,
        //              StatusName = e.Status.StatusName,
        //              LastMessage = o.Message,
        //              LastMessageTime = o.MessageTime,
        //              LastMessageWriter = o.FullName
        //          }
        //         )
        //         .OrderByDescending(d => d.LastMessageTime)
        //         .ToList();


           
        //    return orderlist;

        //}
        //public List<UploadOrdersListViewModel> GetAllOrders(List<int> statuses)
        //{
        //    List<UploadOrdersListViewModel> orderlist = new List<UploadOrdersListViewModel>();
            

        //    List<int> AllStatuses = _datacontext.Statuses.Select(x => x.StatusId).ToList();

        //    List<int> StatusId = new List<int>();
        //    if (statuses != null )
        //    {
        //        StatusId.AddRange(statuses);
        //    }
        //    else
        //    {
        //        StatusId.AddRange(AllStatuses);

        //    }

        //    OrdersMessage EmptyLastMessage = new OrdersMessage()
        //    {
        //        Message = "Немає Повідомлень"
        //    };


       

        //    List<UploadOrdersListViewModel> UserOrdersList = _datacontext.Orders
        //          .Include(x=>x.Db1SOrderNumbers)
        //          .Where(x => StatusId.Contains(x.StatusId))
        //         .Join(
        //         _datacontext.OrdersMessages
        //         .Join(
        //         _datacontext.OrdersMessages.GroupBy(c => c.OrderId)
        //          .Select(v => new
        //          {
        //              OrderId = v.Key,
        //              MessageTime = v.Max(z => z.MessageTime)

        //          }),
        //          q => q.MessageTime,
        //          q2 => q2.MessageTime,
        //          (q, q2) => new
        //          {
        //              q.MessageTime,
        //              q.Message,
        //              q.MessageWriter.FullName,
        //              q.OrderId
        //          }
        //          ),
        //          e => e.OrderId,
        //          o => o.OrderId,
        //          (e, o) => new UploadOrdersListViewModel
        //          {
        //              OrderId = e.OrderId,

        //              CustomerName = e.Customer.FullName,

        //              //ManagerName = e.Manager.Operator.OrderPortalUser.FullName??e.Manager.FullName ,
        //              ManagerName = e.Manager.FullName,
        //              File = e.File,
        //              Db1SOrderNumbers = e.Db1SOrderNumbers.Select(x => x.Db1SOrderNumber).ToList(),

        //              OrderDateComplete = e.OrderDateComplete,
        //              OrderDateCreate = e.OrderDateCreate,
        //              OrderDateProgress = e.OrderDateProgress,
        //              OrderNumber = e.OrderNumber,
        //              OrderNumberContructions = e.OrderNumberContructions,
        //              StatusId = e.StatusId,
        //              StatusName = e.Status.StatusName,
        //              LastMessage = o.Message,
        //              LastMessageTime = o.MessageTime,
        //              LastMessageWriter = o.FullName
        //          }
        //         )
        //         .OrderByDescending(d => d.LastMessageTime)
        //         .ToList();

         
        //    return UserOrdersList;
            

        //}
    }
}