using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrdersPortal.Domain.Entities;

namespace OrdersPortal.Infrastructure.EntityFramework
{
	public class OrderPortalDbContext : IdentityDbContext<OrderPortalUser>
	{
		public OrderPortalDbContext()
			: base("OrdersPortalConnection", throwIfV1Schema: false)
		//: base("OrdersPortalConnection")
		{
		}
		public OrderPortalDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
		{
			Configuration.LazyLoadingEnabled = false;

		}

		public DbSet<Customer> Customers { get; set; }
		public DbSet<RegionManager> RegionManagers { get; set; }
		public DbSet<ComplaintManager> ComplaintManagers { get; set; }
		public DbSet<Manager> Managers { get; set; }
		public DbSet<Operator> Operators { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Director> Directors { get; set; }

		//------------------------------------------------------
		public DbSet<Region> Regions { get; set; }

		public DbSet<Db1SOrderNumbers> Db1SOrdersNumbers { get; set; }
		public DbSet<Query> Queries { get; set; }
		public DbSet<Status> Statuses { get; set; }
		public DbSet<OrdersMessage> OrdersMessages { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Complaint> Complaints { get; set; }
		public DbSet<ComplaintPhoto> ComplaintPhotos { get; set; }
		public DbSet<ComplaintIssue> ComplaintIssues { get; set; }
		public DbSet<ComplaintSolution> ComplaintSolutions { get; set; }
		public DbSet<ComplaintDecision> ComplaintDecisions { get; set; }
		public DbSet<ComplaintOrderSerie> ComplaintOrderSeries { get; set; }
		public DbSet<ComplaintsMessage> ComplaintsMessages { get; set; }
		public DbSet<OrdersPortalLogging> OrdersPortalLogs { get; set; }
		public DbSet<HelpServiceContact> HelpServiceContacts { get; set; }
		public DbSet<HelpServiceLog> HelpServiceLogs { get; set; }
		public DbSet<VideoContent> VideoContents { get; set; }
		public DbSet<UploadFile> Files { get; set; }

		public DbSet<OrderParts> OrderParts { get; set; }
		public DbSet<OrderPartsItem> OrderPartsItems { get; set; }
		public DbSet<Db1SOrderPartsNumbers> Db1SOrderPartsNumbers { get; set; }

		public DbSet<Organization> Organizations { get; set; }
		public DbSet<OrderPortalUserOrganization> OrderPortalUserOrganizations { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // обов’язково викликати базовий метод

			// 🔧 Конфігурація зв’язку OrderPortalUser <-> Organization через проміжну таблицю
			modelBuilder.Entity<OrderPortalUserOrganization>()
				.HasKey(x => new { x.OrderPortalUserId, x.OrganizationId });

			modelBuilder.Entity<OrderPortalUserOrganization>()
				.HasRequired(x => x.OrderPortalUser)
				.WithMany(u => u.OrderPortalUserOrganizations)
				.HasForeignKey(x => x.OrderPortalUserId);

			modelBuilder.Entity<OrderPortalUserOrganization>()
				.HasRequired(x => x.Organization)
				.WithMany(o => o.OrderPortalUserOrganizations)
				.HasForeignKey(x => x.OrganizationId);

			// За потреби можеш додати інші Fluent-конфігурації нижче
		}

	}
}