using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace DataLayer.Helper
{
    public class LogHelper
    {
        public static void writelog(string message)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/Logs/logdata.txt"),message+Environment.NewLine);
        }
    }
}
