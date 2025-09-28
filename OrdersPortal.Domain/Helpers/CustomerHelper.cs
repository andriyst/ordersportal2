
namespace OrdersPortal.Domain.Helpers
{
	public static class CustomerHelper
	{
		public static string GetContrAgentFullCode(string code)
		{
			string fullCode = "000000000";
			return fullCode.Remove(9 - code.Length) + code;
		}
		public static int GetContrAgentShortCode(string code)
		{
			int result = 0;
			int.TryParse(code,out result);
			return result;
		}
	}
}