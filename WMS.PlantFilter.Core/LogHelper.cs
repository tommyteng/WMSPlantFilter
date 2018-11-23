using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.PlantFilter.Core
{
    public class LogHelper
    {
        private static string _LogPath
        {
            get
            {
                string path = string.Empty;
                try
                {
                    path = System.Web.HttpContext.Current.Server.MapPath("~/");
                }
                catch (NullReferenceException)
                {
                    try
                    {
                        path = System.Web.HttpRuntime.AppDomainAppPath;
                    }
                    catch (ArgumentNullException)
                    {
                        path = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }
                return path;
            }
        }


        public static void WriteLog(string message,string preFileName)
        {
            if (string.IsNullOrWhiteSpace(message)) return;


            string logPath = Path.Combine(_LogPath, Constant._LogPath);
            string fileName = string.Format("{0}{1}.log", preFileName, DateTime.Now.ToString("yyyyMMdd"));
          
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            using (FileStream fs = new FileStream(Path.Combine(logPath, fileName), FileMode.Append, FileAccess.Write,
                FileShare.Write, 1024, FileOptions.Asynchronous))
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString("HH:mm:ss") + " " + message + "\r\n");
                IAsyncResult writeResult= fs.BeginWrite(buffer, 0, buffer.Length, t => {
                    var fStream = (FileStream)t.AsyncState;
                    fStream.EndWrite(t);
                }, fs);
            }
        }
    }

    internal class Constant
    {
        public static string _LogPath = WebConfig.GetWebConfig("LogPath", "Logs");
    }
}
