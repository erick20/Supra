using Supra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace Supra.Classes
{
    public class Methods
    {
        public static void Logger(string Message, string StackTrace)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"c:\site_new.out", true))
            {
                sw.WriteLine("DateTime: " + DateTime.Now + " Message: " + Message + "-------" + StackTrace);
                sw.WriteLine(System.Environment.NewLine);
                sw.Close();
            }
            //System.Diagnostics.EventLog.WriteEntry("IdramWebApi", Message + " _*_ " + StackTrace, System.Diagnostics.EventLogEntryType.Error);
        }


        public static RespResult GetErrorDesc(int code, dynamic res)
        {
            ResourceManager rm = new ResourceManager("Supra.Resources.ErrorCodes", Assembly.GetExecutingAssembly());

            RespResult result = new RespResult();

            result.opCode = code;
            result.opDesc = rm.GetString(code.ToString());
            result.RESULT = res;
            return result;
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

    }
}
