using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{

	public class ComplaintsRepository : Repository<Complaint>, IComplaintsRepository
	{
		public ComplaintsRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
		public List<Complaint> GetList()
		{
			return DbSet.ToList();
		}

		public Complaint GetByIdWithIncludes(int complaintId)
		{
			return DbSet.Include(x => x.ComplaintPhoto)
						.Include(x => x.ComplaintOrderSeries)
						.Include(m => m.ComplaintDecisions)
						.Include(x => x.Status)
						.Include(x => x.Customer)
						.Include(x => x.Manager)
						.Include(m => m.ComplaintDecisions.Select(v => v.ComplaintSolution))
						.Include(n => n.ComplaintDecisions.Select(w => w.ComplaintIssue))
						.FirstOrDefault(x => x.ComplaintId == complaintId);
		}

		public Complaint GetByIdWithDecision(int complaintId)
		{
			return DbSet.Include(x => x.ComplaintDecisions).FirstOrDefault(x => x.ComplaintId == complaintId);
		}


		public new IQueryable<Complaint> GetSortAndSearchList(TableDataModel tableDataModel)
		{
			var searchQuery = GetSearchQuery(DbSet.OrderByDescending(x => x.ComplaintDate), (ComplaintTableDataModel)tableDataModel);

			if (!string.IsNullOrEmpty(tableDataModel.Sort))
			{
				searchQuery = GetSortQuery(searchQuery, tableDataModel.Sort, tableDataModel.Order);
			}

			return searchQuery;
		}
		protected IQueryable<Complaint> GetSearchQuery(IQueryable<Complaint> dbSet, ComplaintTableDataModel tableDataModel)
		{
			IQueryable<Complaint> query = dbSet;
			IQueryable<Complaint> result;
			var expressionNew = ExpressionBuilder.True<Complaint>();

			if (!String.IsNullOrEmpty(tableDataModel.Search))
			{
				expressionNew = ExpressionBuilder.False<Complaint>();
				//expressionNew = ExpressionBuilder.Or(expressionNew, x => x.Db1SOrderNumbers.Any(c => c.Db1SOrderNumber.StartsWith(tableDataModel.Search) || c.Db1SOrderNumber.EndsWith(tableDataModel.Search) || c.Db1SOrderNumber.Contains(tableDataModel.Search)));
				expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.Complaint1cOrder, "%" + tableDataModel.Search + "%");
				expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.Customer.FullName, "%" + tableDataModel.Search + "%");
				expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.Manager.FullName, "%" + tableDataModel.Search + "%");
			}

			if (tableDataModel.Statuses != null && tableDataModel.Statuses.Length > 0)
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => tableDataModel.Statuses.Contains(x.StatusId));
			}

			if (tableDataModel.Organizations != null && tableDataModel.Organizations.Any())
			{

				expressionNew = ExpressionBuilder.And(expressionNew, x => x.Customer.OrderPortalUserOrganizations.Any(o => tableDataModel.Organizations.Contains(o.OrganizationId)));
			}

			if (tableDataModel.RegionId != null)
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.Customer.RegionId == tableDataModel.RegionId);
			}

			if (!String.IsNullOrEmpty(tableDataModel.CustomerId))
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.CustomerId == tableDataModel.CustomerId);
			}
			if (!String.IsNullOrEmpty(tableDataModel.ManagerId))
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.ManagerId == tableDataModel.ManagerId);
			}
			if (!String.IsNullOrEmpty(tableDataModel.RegionManagerId))
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.Customer.Customer.RegionManagerId == tableDataModel.RegionManagerId);
			}
			result = query.Where(expressionNew);
			return result;
		}
		protected IQueryable<T> GetSortQuery<T>(IQueryable<T> dbSet, string orderValue, string orderDirection) where T : class
		{
			IQueryable<T> query = dbSet;
			if (!PropertyExists<T>(orderValue))
			{
				return dbSet;
			}

			if (typeof(T).GetProperty(orderValue, BindingFlags.IgnoreCase |
												  BindingFlags.Public | BindingFlags.Instance) == null)
			{
				return null;
			}
			ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
			Expression orderByProperty = Expression.Property(paramterExpression, orderValue);
			LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
			MethodInfo genericMethod;
			if (orderDirection == "desc")
			{
				genericMethod =
					OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
			}
			else
			{
				genericMethod =
					OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
			}
			object ret = genericMethod.Invoke(null, new object[] { query, lambda });
			return (IQueryable<T>)ret;

		}
		private static readonly MethodInfo OrderByMethod =
			typeof(Queryable).GetMethods().Single(method =>
				method.Name == "OrderBy" && method.GetParameters().Length == 2);

		private static readonly MethodInfo OrderByDescendingMethod =
			typeof(Queryable).GetMethods().Single(method =>
				method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

		private static bool PropertyExists<T>(string propertyName)
		{
			return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
													   BindingFlags.Public | BindingFlags.Instance) != null;
		}
	}
}
