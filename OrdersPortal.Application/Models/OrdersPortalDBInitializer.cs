using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Infrastructure.EntityFramework;

namespace OrdersPortal.Application.Models
{
	public class OrdersPortalDBInitializer : DropCreateDatabaseIfModelChanges<OrderPortalDbContext>
	{
		protected override void Seed(OrderPortalDbContext db)
		{
			var UserManager = new UserManager<OrderPortalUser>(new UserStore<OrderPortalUser>(db));
			var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

			db.Regions.Add(new Region { RegionName = "Без Регіону"});
			db.Regions.Add(new Region { RegionName = "Чернівецька обл." });

			db.Statuses.Add(new Status { StatusName = "Завантажено" });
			db.Statuses.Add(new Status { StatusName = "Отримано" });
			db.Statuses.Add(new Status { StatusName = "Обробка" });
            db.Statuses.Add(new Status { StatusName = "Затвердження" });
            db.Statuses.Add(new Status { StatusName = "Оплата" });
			db.Statuses.Add(new Status { StatusName = "Виробництво" });
            db.Statuses.Add(new Status { StatusName = "Відмова" });
            db.Statuses.Add(new Status { StatusName = "Доопрацювання" });
            db.Statuses.Add(new Status { StatusName = "Вирішено" });





            // Додаємо ролі користувачів
            var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
			role.Name = "admin";
			RoleManager.Create(role);

			role.Name = "customer";
			RoleManager.Create(role);
            
			role.Name = "manager";
			RoleManager.Create(role);
            
			role.Name = "regionmanager";
			RoleManager.Create(role);
            
			role.Name = "director";
			RoleManager.Create(role);

		    role.Name = "operator";
            RoleManager.Create(role);


            // Створюємо користувача Admin
            var user = UserManager.FindByName("admin");
			if (user == null)
			{
				user = new OrderPortalUser()
				{
					UserName = "admin",
					FullName = "Admin OrderPortal",
					Email = "cure@viknastyle.com.ua",
					Tel = "3003",
					Enable = true,
                    RegionId = 1,
					Admin = new Admin()
					{
					}
				};

				UserManager.Create(user, "default");

				// Додаємо користувача admin в Роль admin

				if (!UserManager.IsInRole(user.Id, "admin"))
				{
					UserManager.AddToRole(user.Id, "admin");
				}

				//Створюємо запит для відображення кодів контрагентів

				var query = new Query();
				query.QueryText = "ВЫБРАТЬ	Наименование Как ИмяКонтрагента,	Код как КодКонтрагента ИЗ 	Справочник.Контрагенты  ВНУТРЕННЕЕ СОЕДИНЕНИЕ РегистрСведений.КатегорииОбъектов	ПО (Ссылка = (ВЫРАЗИТЬ(Объект КАК Справочник.Контрагенты))) ГДЕ НЕ ЭтоГруппа	И НЕ ПометкаУдаления	И Категория.Код = \"000000196\" УПОРЯДОЧИТЬ ПО Наименование АВТОУПОРЯДОЧИВАНИЕ";
				query.QueryName = "CustomersList";
				query.QueryDesc = "Список контрагентів з їх Кодами";
				db.Queries.Add(query);

				// Зберігаємо
				db.SaveChanges();

				base.Seed(db);

				//UserManager.Dispose();
				//RoleManager.Dispose();

			}



		}
	}
}