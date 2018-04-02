using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logistics.Entity.AliYiYuan;
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
    ///阿里易源数据 物流接口
    ///https://market.aliyun.com/products/57126001/cmapi010996.html?spm=5176.730005.0.0.dO4BoH#sku=yuncode499600008
    ///官方文档
    /// https://www.showapi.com/api/lookPoint/64
    /// </summary>
    public class AliYiYuanService
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="Para"></param>
        /// <returns></returns>
        public Config.ResultObj Service(Config.GetPara Para)
        {
            const String host = "https://ali-deliver.showapi.com";
            const String path = "/showapi_expInfo";
            const String method = "GET";
            const String appcode = "42b6f1042f8945d78ca59be867a39f5a";
            Config.ResultObj ret = new Config.ResultObj();
            #region##参数验证
            if (Para == null)
            {
                ret.showapi_res_code = 4;
                ret.showapi_res_error = "缺少请求参数";
                return ret;
            }
            if (string.IsNullOrWhiteSpace(Para.com))
            {
                ret.showapi_res_code = 4;
                ret.showapi_res_error = "缺少快递公司参数";
                return ret;
            }
            if (string.IsNullOrWhiteSpace(Para.nu))
            {
                ret.showapi_res_code = 4;
                ret.showapi_res_error = "缺少快递单号参数参数";
                return ret;
            }
            #endregion
            #region###开始处理
            String querys ="nu=" + Para.nu+ "&com="+ "auto";
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
            Normal.WritLog("参数："+JsonConvert.SerializeObject(Para));
            Normal.WritLog("查询返回：" + ResultJson);
            if (!string.IsNullOrWhiteSpace(ResultJson))
            {
                ret = JsonConvert.DeserializeObject<Config.ResultObj>(ResultJson);
            }
            else
            {
                ret.showapi_res_code = 4; ret.showapi_res_error = "暂无返回数据";
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

        public ExpressData ConvertToExpressData(Config.ResultObj ret)
        {
            ExpressData Result = new ExpressData();
            ///物流信息获取成功
            if (ret != null && ret.showapi_res_code == 0 && ret.showapi_res_body.flag && ret.showapi_res_body.ret_code == 0)
            {
                List<ExpressDataItem> list=new List<ExpressDataItem>();
                Result.Success = true;
                Result.Message = "物流获取成功";
                Result.ApiName = "AliYiYuan";
                Result.date = DateTime.Now;
                Result.number = ret.showapi_res_body.mailNo;
                Result.type = ret.showapi_res_body.expSpellName;
                if (ret.showapi_res_body.status == 2)
                {
                    Result.Status = 1;
                }
                else if (ret.showapi_res_body.status == 3)
                {
                    Result.Status = 2;
                }
                else if (ret.showapi_res_body.status == 4)
                {
                    Result.Status = 3;
                }
                else if (ret.showapi_res_body.status == 5)
                {
                    Result.Status = 4;
                }
                else
                {
                    Result.Status = 0;
                }
                foreach (Config.showapi_res_Item Item in ret.showapi_res_body.data)
                {
                    list.Add(new ExpressDataItem() {
                        Content = Item.context,
                        Time = DateTime.Parse(Item.time),
                    });
                }
                var Temp = list.OrderByDescending(P=>P.Time).ToList();
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
