using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logistics.Entity.AliFuQing;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Logistics.Entity;
using Logistics.Common;
using Logistics.Data;

namespace Logistics.Service
{
    /// <summary>
    ///涪擎数据 物流接口
    ///https://market.aliyun.com/products/56928004/cmapi021863.html?spm=5176.730005.0.0.xmkqtR#sku=yuncode1586300000
    ///官方文档
    /// https://market.aliyun.com/products/56928004/cmapi021863.html?spm=5176.730005.0.0.xmkqtR#sku=yuncode1586300000
    /// </summary>
    public class FuQingService
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="Para"></param>
        /// <returns></returns>
        public Config_Fuqing.ResultObj Service(Config_Fuqing.GetPara Para)

        {
            const String host = "https://wuliu.market.alicloudapi.com";
            const String path = "/kdi";
            const String method = "GET";
            const String appcode = "42b6f1042f8945d78ca59be867a39f5a";
            Config_Fuqing.ResultObj ret = new Config_Fuqing.ResultObj();
            #region##参数验证
            if (Para == null)
            {
                ret.status = 4;
                ret.msg = "缺少请求参数";
                return ret;
            }
            if (string.IsNullOrWhiteSpace(Para.type))
            {
                ret.status = 4;
                ret.msg = "缺少快递公司参数";
                //return ret;
            }
            if (string.IsNullOrWhiteSpace(Para.no))
            {
                ret.status = 4;
                ret.msg = "缺少快递单号参数参数";
                return ret;
            }
            #endregion
            #region###开始处理
            String querys = "no=" + Para.no+ "&type="+"";
            String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            //Console.WriteLine(httpResponse.StatusCode);
            //Console.WriteLine(httpResponse.Method);
            //Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            string ResultJson = reader.ReadToEnd();
            Normal.WritLog("参数：" + JsonConvert.SerializeObject(Para));
            Normal.WritLog("查询返回：" + ResultJson);
            if (!string.IsNullOrWhiteSpace(ResultJson))
            {
                ret = JsonConvert.DeserializeObject<Config_Fuqing.ResultObj>(ResultJson);
            }
            else
            {
                ret.status = 4; ret.msg = "暂无返回数据";
            }
            //Console.WriteLine(reader.ReadToEnd());
            //Console.WriteLine("\n");
            #endregion
            return ret;
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        /// <summary>
        /// 物流信息转为商城通用格式
        /// </summary>
        /// <param name="ret"></param>
        /// <returns></returns>

        public ExpressData ConvertToExpressData(Config_Fuqing.ResultObj ret)
        {
            ExpressData Result = new ExpressData();
            ///物流信息获取成功
            if (ret != null && ret.status == 0)
            {
                List<ExpressDataItem> list = new List<ExpressDataItem>();
                Result.Success = true;
                Result.Message = "物流获取成功";
                Result.ApiName = "FuQing";
                Result.Status = ret.result.deliverystatus;
                Result.date = DateTime.Now;
                Result.number = ret.result.number;
                Result.type = ret.result.type;
                foreach (Config_Fuqing.result_Item Item in ret.result.list)
                {
                    list.Add(new ExpressDataItem()
                    {
                        Content = Item.status,
                        Time = DateTime.Parse(Item.time),
                    });
                }
                var Temp = list.OrderByDescending(P => P.Time).ToList();
                Result.ExpressDataItems = Temp;
            }
            else
            {
                Result.Success = false;
                Result.Message = "物流获取失败";
            }
            return Result;
        }
    }
}
