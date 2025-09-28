using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace OrdersPortal.Domain.Helpers
{
	public static class ExpressionBuilder
	{
		public static Expression<Func<T, bool>> True<T>() { return f => true; }
		public static Expression<Func<T, bool>> False<T>() { return f => false; }

		public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
			Func<Expression, Expression, Expression> merge)
		{
			var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
			               .ToDictionary(p => p.s, p => p.f);

			Expression secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

			return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
		}

		public static Expression<Func<T, bool>> And<T>(
			this Expression<Func<T, bool>> first,
			Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.And);
		}

		public static Expression<Func<T, bool>> Or<T>(
			this Expression<Func<T, bool>> first,
			Expression<Func<T, bool>> second)
		{
			return first.Compose(second, Expression.Or);
		}

		public class ParameterRebinder : ExpressionVisitor
		{
			private readonly Dictionary<ParameterExpression, ParameterExpression> map;

			public ParameterRebinder(
				Dictionary<ParameterExpression,
					ParameterExpression> map)
			{
				this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
			}

			public static Expression ReplaceParameters(
				Dictionary<ParameterExpression,
					ParameterExpression> map,
				Expression exp)
			{
				return new ParameterRebinder(map).Visit(exp);
			}

			protected override Expression VisitParameter(ParameterExpression p)
			{
				ParameterExpression replacement;
				if (map.TryGetValue(p, out replacement))
				{
					p = replacement;
				}
				return base.VisitParameter(p);
			}
		}

		public static Expression<Func<T, bool>> Like<T>(Expression<Func<T, string>> expr, string likeValue)
		{
			var paramExpr = expr.Parameters.First();
			var memExpr = expr.Body;

			if (likeValue == null || likeValue.Contains('%') != true)
			{
				Expression<Func<string>> valExpr = () => likeValue;
				var eqExpr = Expression.Equal(memExpr, valExpr.Body);
				return Expression.Lambda<Func<T, bool>>(eqExpr, paramExpr);
			}

			if (likeValue.Replace("%", string.Empty).Length == 0)
			{
				return ExpressionBuilder.True<T>();
			}

			likeValue = Regex.Replace(likeValue, "%+", "%");

			if (likeValue.Length > 2 && likeValue.Substring(1, likeValue.Length - 2).Contains('%'))
			{
				likeValue = likeValue.Replace("[", "[[]").Replace("_", "[_]");
				Expression<Func<string>> valExpr = () => likeValue;
				var patExpr = Expression.Call(typeof(SqlFunctions).GetMethod("PatIndex",
					new[] { typeof(string), typeof(string) }), valExpr.Body, memExpr);
				var neExpr = Expression.NotEqual(patExpr, Expression.Convert(Expression.Constant(0), typeof(int?)));
				return Expression.Lambda<Func<T, bool>>(neExpr, paramExpr);
			}

			if (likeValue.StartsWith("%"))
			{
				if (likeValue.EndsWith("%") == true)
				{
					likeValue = likeValue.Substring(1, likeValue.Length - 2);
					Expression<Func<string>> valExpr = () => likeValue;
					var containsExpr = Expression.Call(memExpr, typeof(String).GetMethod("Contains",
						new[] { typeof(string) }), valExpr.Body);
					return Expression.Lambda<Func<T, bool>>(containsExpr, paramExpr);
				}
				else
				{
					likeValue = likeValue.Substring(1);
					Expression<Func<string>> valExpr = () => likeValue;
					var endsExpr = Expression.Call(memExpr, typeof(String).GetMethod("EndsWith",
						new[] { typeof(string) }), valExpr.Body);
					return Expression.Lambda<Func<T, bool>>(endsExpr, paramExpr);
				}
			}
			else
			{
				likeValue = likeValue.Remove(likeValue.Length - 1);
				Expression<Func<string>> valExpr = () => likeValue;
				var startsExpr = Expression.Call(memExpr, typeof(String).GetMethod("StartsWith",
					new[] { typeof(string) }), valExpr.Body);
				return Expression.Lambda<Func<T, bool>>(startsExpr, paramExpr);
			}
		}

		public static Expression<Func<T, bool>> AndLike<T>(this Expression<Func<T, bool>> predicate, Expression<Func<T, string>> expr, string likeValue)
		{
			var andPredicate = Like(expr, likeValue);
			if (andPredicate != null)
			{
				//	predicate = predicate.And(andPredicate.Compose());
				predicate = predicate.Compose(andPredicate, Expression.And);
			}
			return predicate;
		}

		public static Expression<Func<T, bool>> OrLike<T>(this Expression<Func<T, bool>> predicate, Expression<Func<T, string>> expr, string likeValue)
		{
			var orPredicate = Like(expr, likeValue);
			if (orPredicate != null)
			{
				predicate = predicate.Compose(orPredicate, Expression.Or);
			}
			return predicate;
		}
	}
}