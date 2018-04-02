using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Logistics.Common
{
  public  class Normal
    {
        /// <summary>
        /// 获取用户IP
        /// </summary>
        /// <returns></returns>
        public static string GetWebClientIp()
        {
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
            {
                return "0.0.0.0";
            }

            HttpRequest Request = HttpContext.Current.Request;

            string UserIP = Request.UserHostAddress;
            if (Request.Headers["X-Forwarded-For"] != null)
            {
                UserIP = Request.Headers["X-Forwarded-For"].ToString();
            }
            else if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                UserIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }

            else if (Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                UserIP = Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                UserIP = Request.UserHostAddress.ToString();
            }
            Regex IPRegex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!IPRegex.IsMatch(UserIP))
            {
                UserIP = Request.UserHostAddress.ToString();
            }

            return UserIP;

        }


        /// <summary>
        /// 输入日志
        /// </summary>
        /// <param name="mess"></param>
        public static void WritLog(string mess)
        {
            //string path = System.Web.HttpContext.Current.Server.MapPath("tokenlog.txt");
            try
            {
                string path = System.Web.HttpRuntime.AppDomainAppPath + "tokenlog.txt";
                System.IO.File.AppendAllText(path, "\r\n" + DateTime.Now.ToString() + mess, Encoding.UTF8);

            }
            catch (IOException ex)
            {
                //System.IO.IOException 文件占用异常
                //忽略此异常
            }
            catch (Exception e) { }
        }

    }
}
