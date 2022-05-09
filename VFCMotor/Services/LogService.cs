using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFCMotor
{
   public class LogService
    {
        public static string Password = "";
        public static void LogMessage(string msg)
        {
            string sFilePath = Environment.CurrentDirectory + "\\"  +  "Log.txt";

            System.IO.StreamWriter sw = System.IO.File.AppendText(sFilePath);
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine,Encoding.GetEncoding("UTF-8"));
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
