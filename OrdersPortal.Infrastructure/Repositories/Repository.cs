using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OrdersPortal.Domain.Helpers;
using OrdersPortal.Domain.Repositories;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class Repository<T> : RepositoryBase, IRepository<T> where T : class /*, IEntity*/
	{
		public DbSet<T> DbSet { get; private set; }



		public Repository(UnitOfWork unitOfWork)
			: base(unitOfWork)
		{
			DbSet = unitOfWork.DbContext.Set<T>();
		}

		public IQueryable<T> GetAll()
		{
			return DbSet;
		}


		public T GetById(object id)
		{
			return DbSet.Find(id);
		}

		public virtual void Add(T newEntity)
		{
			DbSet.Add(newEntity);
		}

		public virtual void AddPermanent(T newEntity)
		{
			DbSet.Add(newEntity);
			UnitOfWork.Commit();
		}

		public virtual void Remove(T entity)
		{
			DbSet.Remove(entity);
		}

		public virtual void Update(T entity)
		{
			UnitOfWork.DbContext.Entry(entity).State = EntityState.Modified;
		}

		public virtual void UpdatePermanent(T entity)
		{
			UnitOfWork.DbContext.Entry(entity).State = EntityState.Modified;
			UnitOfWork.Commit();
		}

		public virtual void SaveChanges()
		{
			UnitOfWork.Commit();
		}

		public virtual IQueryable<T> GetSortAndSearchList(TableDataModel tableDataModel)
		{
			IQueryable<T> searchQuery;
			if (string.IsNullOrEmpty(tableDataModel.Search))
			{
				searchQuery = DbSet;
			}
			else
			{
				searchQuery = GetSearchQuery(DbSet, tableDataModel.Search);
			}
			if (!string.IsNullOrEmpty(tableDataModel.Sort))
			{
				searchQuery = GetSortQuery(searchQuery, tableDataModel.Sort, tableDataModel.Order);
			}

			return searchQuery;

		}

		protected IQueryable<T> GetSearchQuery<T>(IQueryable<T> dbSet, string value) where T : class
		{
			Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();

			List<string> nestedFileds = GetTypeProperties<T>(out propertyInfos);

			IQueryable<T> query = dbSet;

			List<Expression> expressions = new List<Expression>();

			ParameterExpression parameter = Expression.Parameter(typeof(T), "p");

			foreach (PropertyInfo prop in typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string)))
			{
				var containsExpression = CreateExpression(typeof(T), prop.Name, value, parameter);
				expressions.Add(containsExpression);
			}

			foreach (var field in nestedFileds)
			{
				var exp = CreateExpression(typeof(T), field, value, parameter);
				expressions.Add(exp);
			}


			IQueryable<T> result;

			if (expressions.Count == 0)
				result = query;
			else
			{

				Expression orExpression = expressions[0];

				Expression<Func<T, bool>> expressionNew = Expression.Lambda<Func<T, bool>>(
					orExpression, parameter);

				for (int i = 1; i < expressions.Count; i++)
				{

					expressionNew = OrElse<T>(expressionNew, Expression.Lambda<Func<T, bool>>(
								 expressions[i], parameter));
				}

				result = query.Where(expressionNew);
			}

			return result;
		}

		public static Expression<Func<T, bool>> OrElse<T>(
		Expression<Func<T, bool>> expr1,
	   Expression<Func<T, bool>> expr2)
		{
			var parameter = Expression.Parameter(typeof(T));

			var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
			var left = leftVisitor.Visit(expr1.Body);

			var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
			var right = rightVisitor.Visit(expr2.Body);

			return Expression.Lambda<Func<T, bool>>(
				Expression.OrElse(left, right), parameter);
		}

		private class ReplaceExpressionVisitor
	   : ExpressionVisitor
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
		private static MethodCallExpression CreateExpression(Type type, string propertyName, string propertyValue, ParameterExpression param)
		{
			MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
			var someValue = Expression.Constant(propertyValue, typeof(string));
			//var param = Expression.Parameter(type, "x");

			Expression body = param;

			foreach (var member in propertyName.Split('.'))
			{
				body = Expression.PropertyOrField(body, member);

			}

			var containsMethodExp = Expression.Call(body, containsMethod, someValue);

			return containsMethodExp;
			//  return Expression.Lambda(containsMethodExp, param);
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

		public static Expression<Func<TEntity, bool>> CreateSearchQuery<TEntity>(List<PropertyInfo> properties, string text)
		{
			if (string.IsNullOrWhiteSpace(text) || properties == null || properties.Count == 0)
			{
				return null;
			}

			// For comparison
			//Expression<Func<ProductContainer, bool>> exp = q => searchText.Any(x => q.Product.ProductTitle.ToLower().Contains(x));

			var expressions = new List<Expression>();

			var entity = Expression.Parameter(typeof(TEntity), "q");

			//search type
			var searchMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

			//search terms
			var searchTerms = Expression.Constant(text.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

			//search param
			var str = Expression.Parameter(typeof(string), "s");

			foreach (var property in properties.Where(
				x => x.GetCustomAttribute<NotMappedAttribute>() == null))
			{

				var entityProperty = Expression.Property(entity, property);
				var toLower = Expression.Call(entityProperty, "ToLower", Type.EmptyTypes);
				var contains = Expression.Call(toLower, searchMethod, str);

				var anyExpression = Expression.Lambda<Func<string, bool>>(contains, str);

				var any = Expression.Call(typeof(Enumerable), "Any", new[] { typeof(string) }, searchTerms, anyExpression);

				expressions.Add(any);
			}

			var ors = expressions.Aggregate((x, y) => Expression.Or(x, y));

			var exp = Expression.Lambda<Func<TEntity, bool>>(ors, entity);
			return exp;
		}
		private static List<string> GetTypeProperties<T>(out Dictionary<string, PropertyInfo> commonProperties) where T : class
		{
			var existedProperties = typeof(T)
				.GetProperties()
				.ToList()
				.ToDictionary(p => p.Name);

			commonProperties = existedProperties;

			List<string> result = new List<string>();
			var additionaProperties = new Dictionary<string, PropertyInfo>();

			foreach (var property in existedProperties)
			{

				if (!property.Value.PropertyType.IsValueType
						 && !property.Value.PropertyType.IsPrimitive
						 && (property.Value.PropertyType.Namespace != null && property.Value.PropertyType.Namespace.Contains("OrdersPortal2"))
						 )
				{
					var myType1 = Type.GetType(String.Concat((property.Value.PropertyType.Namespace + "." + property.Key), ", ", "OrdersPortal.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"));
					if (myType1 != null)
					{
						additionaProperties = myType1.GetProperties().ToList().ToDictionary(p => p.Name);

						foreach (var addProp in additionaProperties.Where(x => x.Value.PropertyType.Name.Contains("String")))
						{
							result.Add(myType1.Name + '.' + addProp.Key);
						}
					}
				}

			}

			return result;


		}
	}
}
