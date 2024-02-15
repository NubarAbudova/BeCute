using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Utilities.Extensions
{
	public static class AppUserValidator
	{
		public static string Capitalize(this string String)
		{
			return String.Trim().Substring(0, 1).ToUpper() + String.ToUpper().Substring(1).ToLower();
		}

		public static bool CheeckEmail(this string String)
		{
			string pattern = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$";
			Regex regex = new Regex(pattern);
			if (regex.IsMatch(String))
			{
				return true;
			}
			return false;
		}
		public static bool IsLetter(this string String)
		{
			bool result = false;

			foreach (Char letter in String)
			{
				result = Char.IsLetter(letter);
			}
			return result;
		}

	}
}
