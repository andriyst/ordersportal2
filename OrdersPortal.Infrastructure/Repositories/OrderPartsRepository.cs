using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OrdersPortal.Domain.Entities;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{

	public class OrderPartsRepository : Repository<OrderParts>, IOrderPartsRepository
	{
		public OrderPartsRepository(UnitOfWork unitOfWork) : base(unitOfWork)
		{

		}
		public List<OrderParts> GetList()
		{
			return DbSet.ToList();
		}
		//public string GetFilePath(int id)
		//{
		//	OrderParts ordeParts = DbSet.FirstOrDefault(x => x.OrderPartsId == id);
		//	return orderParts?.File;
		//}

		public new IQueryable<OrderParts> GetSortAndSearchList(TableDataModel tableDataModel)
		{
			var searchQuery = GetSearchQuery(DbSet.AsNoTracking().OrderByDescending(x => x.OrderPartsDate), (OrderPartsTableDataModel)tableDataModel);

			if (!string.IsNullOrEmpty(tableDataModel.Sort))
			{
				searchQuery = GetSortQuery(searchQuery, tableDataModel.Sort, tableDataModel.Order);
			}

			return searchQuery;
		}
	
		public string GetOrderPartsNumber(string userId)
		{
			List<string> customerOrders = DbSet.Where(x => x.CustomerId == userId).Select(x => x.OrderNumber).ToList();

			int numOrderPartsNumber = customerOrders.Count;

			while (customerOrders.Contains(numOrderPartsNumber.ToString()))
			{
				numOrderPartsNumber++;
			}

			return numOrderPartsNumber.ToString();
		}



		//public OrderParts GetOrderPartsByDb1cOrderPartsNumber(string db1COrderPartsNumber)
		//{
		//	//return DbSet.Include(x => x.Db1SOrderPartsNumber).FirstOrDefault(x => x.Db1SOrderPartsNumber.Select(y => y.Db1SOrderPartsNumbers).Contains(db1COrderPartsNumber));
		//	// return DbSet.Include(x => x.Db1SOrderPartsNumber).FirstOrDefault(x => x.Db1SOrderPartsNumber.Db1SOrderPartsNumber == db1COrderPartsNumber);
		//	return DbSet.Include(x => x.Db1SOrderPartsNumber).FirstOrDefault(x => x.Db1SOrderPartsNumber.Db1SOrderPartsNumber == db1COrderPartsNumber);
		//}
		
		#region Private methods


		private IQueryable<OrderParts> GetSearchQuery(IQueryable<OrderParts> dbSet, OrderPartsTableDataModel tableDataModel)
		{
			IQueryable<OrderParts> query = dbSet;
			IQueryable<OrderParts> result;
			var expressionNew = ExpressionBuilder.True<OrderParts>();

			if (!String.IsNullOrEmpty(tableDataModel.Search))
			{
				expressionNew = ExpressionBuilder.False<OrderParts>();
				// expressionNew = ExpressionBuilder.Or(expressionNew, x => x.Db1SOrderPartsNumber.Any(c => c.Db1SOrderPartsNumber.StartsWith(tableDataModel.Search) || c.Db1SOrderPartsNumber.EndsWith(tableDataModel.Search) || c.Db1SOrderPartsNumber.Contains(tableDataModel.Search)));
				// expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.Db1SOrderPartsNumber.Db1SOrderPartsNumber, "%" + tableDataModel.Search + "%");
				expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.OrderNumber, "%" + tableDataModel.Search + "%");
				expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.Customer.FullName, "%" + tableDataModel.Search + "%");
				expressionNew = ExpressionBuilder.OrLike(expressionNew, x => x.Manager.FullName, "%" + tableDataModel.Search + "%");
			}

			if (tableDataModel.Statuses != null && tableDataModel.Statuses.Length > 0)
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => tableDataModel.Statuses.Contains(x.StatusId));
			}

			if (!string.IsNullOrEmpty(tableDataModel.ManagerId))
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.Customer.Customer.ManagerId == tableDataModel.ManagerId);
			}
			if (!string.IsNullOrEmpty(tableDataModel.RegionManagerId))
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.Customer.Customer.RegionManagerId == tableDataModel.RegionManagerId);
			}

			if (!String.IsNullOrEmpty(tableDataModel.CustomerId))
			{
				expressionNew = ExpressionBuilder.And(expressionNew, x => x.CustomerId == tableDataModel.CustomerId);
			}
			result = query.Where(expressionNew);
			return result;
		}

		private new static Expression<Func<T, bool>> OrElse<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var parameter = Expression.Parameter(typeof(T));

			var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
			var left = leftVisitor.Visit(expr1.Body);

			var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
			var right = rightVisitor.Visit(expr2.Body);

			return Expression.Lambda<Func<T, bool>>(
				Expression.OrElse(left, right), parameter);
		}


		private class ReplaceExpressionVisitor : ExpressionVisitor
		{
			private readonly Expression _oldValue;
			private readonly Expression _newValue;

			public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
			{
				_oldValue = oldValue;
				_newValue = newValue;
			}

			public override Expression Visit(Expression node)
			{
				if (node == _oldValue)
					return _newValue;
				return base.Visit(node);
			}
		}
		protected new IQueryable<T> GetSortQuery<T>(IQueryable<T> dbSet, string orderValue, string orderDirection) where T : class
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

		#endregion
	}
}
