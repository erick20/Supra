using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
