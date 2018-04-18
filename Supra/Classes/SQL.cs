using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supra.Classes
{
    public class SQL
    {
        /// <summary>
        /// Replaces ' with '' to be used in SQL query
        /// </summary>
        /// <param name="sStr">Input string</param>
        /// <returns>Encoded String</returns>
        public static string String(Object sStr)
        {
            //sStr = Regex.Replace(sStr.ToString(), "`<(.| )*?>`", "", RegexOptions.Singleline);
            return (sStr == null ? "''" : "'" + ("" + sStr).Replace("'", "''") + "'");
        }

        public static string Number(Object sStr)
        {
            try
            {
                return ("" + Convert.ToDecimal("" + sStr));
            }
            catch { }

            return "NULL";
        }

        public static bool IsNumeric(string s)
        {
            bool isNumeric = true;
            foreach (Char c in s.ToCharArray())
            {
                isNumeric = isNumeric && Char.IsDigit(c);
            }

            return isNumeric;
        }

        public static string Number(string sValue, string sFormat)
        {
            try
            {
                return Convert.ToDecimal("" + sValue).ToString(sFormat);
            }
            catch { }

            return "";
        }

        public static string DateTime(string sDate, string sFormat)
        {
            if (sDate == "") return "NULL";

            try
            {
                return "'" + System.DateTime.ParseExact(sDate, sFormat, null).ToString("yyyyMMdd HH:mm:ss") + "'";
            }
            catch { }

            return "NULL";
        }

        public static string DateTime(DateTime dbDate)
        {
            return "'" + dbDate.ToString("yyyyMMdd HH:mm:ss") + "'";
        }

        public static string DateTime(object dbDate, int iTimeZone)
        {
            if (dbDate is DBNull)
                return "NULL";
            else
                iTimeZone = 0;
            return "'" + System.DateTime.ParseExact(dbDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).AddMinutes(iTimeZone).ToString("yyyyMMdd HH:mm:ss") + "'";

            //return "'" + (Convert.ToDateTime(dbDate)).AddMinutes(iTimeZone).ToString("yyyyMMdd HH:mm:ss") + "'";
            //    return "'"+ (Convert.ToDateTime( dbDate)).ToString("yyyyMMdd HH:mm:ss") + "'";
        }
    }

    
    
}
