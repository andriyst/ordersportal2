using System;
using System.Data.SqlClient;

namespace OrdersPortal.Infrastructure.Repositories
{
	public class RepositoryBase
	{
		protected UnitOfWork UnitOfWork { get; private set; }

		public RepositoryBase(UnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;

		}

		protected SqlConnection GetConnection()
		{
			return new SqlConnection(UnitOfWork.ConnectionString);
		}

		public static object GetNullableDbValue(object value)
		{

			if (value == DBNull.Value)
				return null;


			return value;
		}

		public static string GetString(object value)
		{
			string stringValue = (string)GetNullableDbValue(value);
			if (stringValue == null)
			{
				return "";
			}
			return stringValue.Trim();
		}

		public static SByte GetSByte(object value)
		{
			object byteValue = GetNullableDbValue(value);

			if (byteValue == null)
			{
				return 0;
			}
			return Convert.ToSByte(byteValue);
		}

		public static byte GetByte(object value)
		{
			object byteValue = GetNullableDbValue(value);

			if (byteValue == null)
			{
				return 0;
			}
			return Convert.ToByte(byteValue);
		}

		public static int GetInteger(object value)
		{
			object integerValue = GetNullableDbValue(value);

			if (integerValue == null)
			{
				return 0;
			}
			return (int)integerValue;
		}

		public static long GetLong(object value)
		{
			object longValue = GetNullableDbValue(value);

			if (longValue == null)
			{
				return 0;
			}
			return (long)longValue;
		}

		public static double GetDouble(object value)
		{
			object doubleValue = GetNullableDbValue(value);

			if (doubleValue == null)
			{
				return 0;
			}
			return (double)doubleValue;
		}

		public static decimal GetDecimal(object value)
		{
			object decimalValue = GetNullableDbValue(value);

			if (decimalValue == null)
			{
				return 0;
			}
			return (decimal)decimalValue;
		}

		public static bool GetBoolean(object value)
		{
			object boolValue = GetNullableDbValue(value);

			if (boolValue == null)
			{
				return false;
			}
			return (bool)boolValue;
		}

		public static DateTime GetDateTime(object value)
		{
			object dateTimeValue = GetNullableDbValue(value);

			if (dateTimeValue == null)
			{
				return DateTime.MinValue;
			}
			return (DateTime)dateTimeValue;
		}

		public static Guid GetGuid(object value)
		{
			object guidValue = GetNullableDbValue(value);

			if (guidValue == null)
			{
				return Guid.Empty;
			}
			return (Guid)guidValue;
		}

		public static string GetNullableString(object value)
		{
			string stringValue = (string)GetNullableDbValue(value);
			if (stringValue == null)
			{
				return null;
			}
			return stringValue.Trim();
		}

		public static SByte? GetNullableSByte(object value)
		{
			object byteValue = GetNullableDbValue(value);

			if (byteValue == null)
			{
				return null;
			}
			return Convert.ToSByte(byteValue);
		}

		public static byte? GetNullableByte(object value)
		{
			object byteValue = GetNullableDbValue(value);

			if (byteValue == null)
			{
				return null;
			}
			return Convert.ToByte(byteValue);
		}

		public static int? GetNullableInteger(object value)
		{
			object integerValue = GetNullableDbValue(value);

			if (integerValue == null)
			{
				return null;
			}
			return (int)integerValue;
		}

		public static long? GetNullableLong(object value)
		{
			object longValue = GetNullableDbValue(value);

			if (longValue == null)
			{
				return null;
			}
			return (long)longValue;
		}

		public static double? GetNullableDouble(object value)
		{
			object doubleValue = GetNullableDbValue(value);

			if (doubleValue == null)
			{
				return null;
			}
			return (double)doubleValue;
		}

		public static decimal? GetNullableDecimal(object value)
		{
			object decimalValue = GetNullableDbValue(value);

			if (decimalValue == null)
			{
				return null;
			}
			return (decimal)decimalValue;
		}

		public static bool? GetNullableBoolean(object value)
		{
			object boolValue = GetNullableDbValue(value);

			if (boolValue == null)
			{
				return null;
			}
			return (bool)boolValue;
		}

		public static DateTime? GetNullableDateTime(object value)
		{
			object dateTimeValue = GetNullableDbValue(value);

			if (dateTimeValue == null)
			{
				return null;
			}
			return (DateTime)dateTimeValue;
		}

		public static Guid? GetNullableGuid(object value)
		{
			object guidValue = GetNullableDbValue(value);

			if (guidValue == null)
			{
				return null;
			}
			return (Guid)guidValue;
		}


	}
}
