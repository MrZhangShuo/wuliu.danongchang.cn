using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistics.Service;
using Logistics.Entity;
using System.IO;
using Newtonsoft.Json;
using Logistics.Common;
using Logistics.Data;
using Logistics.Common.CacheManager;

namespace Logistics.Web.Controllers
{
    public class LogisticsAPIController : Controller
    {

        public bool IsDebug = false;
        // GET: /API/
        public ActionResult Index()
        {
            //涪擎本地测试
            //Logistics.Entity.AliFuQing.Config_Fuqing.GetPara Para = new Logistics.Entity.AliFuQing.Config_Fuqing.GetPara();
            //Para.type = "SFEXPRESS";
            //Para.no = "964384723706";
            //Para.AppCode = "42b6f1042f8945d78ca59be867a39f5a";
            //new FuQingService().Service(Para);  

            //易源测试
            //Logistics.Entity.AliYiYuan.Config.GetPara Para = new Logistics.Entity.AliYiYuan.Config.GetPara();
            //Para.com = "shentong";
            //Para.nu = "3355965336893";
            //Para.AppCode = "42b6f1042f8945d78ca59be867a39f5a";
            //new AliYiYuanService().Service(Para);
            return View();
        }
        /// <summary>
        /// 外部调用api接口信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Query()
        {
            LogisticPara LogisticPara = null;
            ExpressData ExpressData = new ExpressData();
            ExpressData.Success = false;
            ExpressData.Message = "获取失败";
            //获取Post参数
            Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(postData);
            string postContent = sRead.ReadToEnd();
            sRead.Close();

            if (IsDebug)
            {
                Normal.WritLog("接受到的参数_postContent：" + postContent);
            }
            if (string.IsNullOrWhiteSpace(postContent))
            {
                ExpressData.Success = false; ExpressData.Message = "缺少参数";
                return View();
            }
            try
            {
                LogisticPara = JsonConvert.DeserializeObject<LogisticPara>(postContent);
            }
            catch (Exception ex)
            {
                ExpressData.Success = false; ExpressData.Message = "缺少参数";
                return View();
            }
            if (LogisticPara == null)
            {
                ExpressData.Success = false;
                ExpressData.Message = "缺少参数";
                return View();
            }
            //验证物流单号
            if (string.IsNullOrWhiteSpace(LogisticPara.logisticsCode))
            {
                ExpressData.Success = false; ExpressData.Message = "缺少物流单号参数"; return View();
            }
            if (string.IsNullOrWhiteSpace(LogisticPara.shipperCode))
            {
                ExpressData.Success = false; ExpressData.Message = "缺少物流公司代码"; return View();
            }
            ManagerCache theManagerCache = new ManagerCache(true);
            //如果缓存没有信息
            if (!theManagerCache.CacheService.IsSet(LogisticPara.logisticsCode))
            {
                ExpressData = DataQuery.Query_WuliuMess(LogisticPara.logisticsCode);//从数据库取出 
                bool isNewItem = false;
                if (ExpressData == null? isNewItem = true : isNewItem = false || ExpressData.Status == 1 || ExpressData.Status == 2)
                {
                    #region###涪擎物流接口
                    Logistics.Entity.AliFuQing.Config_Fuqing.GetPara Para_fuqing = new Logistics.Entity.AliFuQing.Config_Fuqing.GetPara();
                    Para_fuqing.type = LogisticPara.shipperCode;
                    Para_fuqing.no = LogisticPara.logisticsCode;
                    Para_fuqing.AppCode = LogisticPara.KuaidiApp_Code;
                    ExpressData = new FuQingService().ConvertToExpressData(new FuQingService().Service(Para_fuqing));
                    #endregion
                    //如果查询失败转向下一接口
                    if (ExpressData.Success == false)
                    {
                        #region###易源物流接口
                        Logistics.Entity.AliYiYuan.Config.GetPara Para = new Logistics.Entity.AliYiYuan.Config.GetPara();
                        Para.com = LogisticPara.shipperCode;
                        Para.nu = LogisticPara.logisticsCode;
                        Para.AppCode = LogisticPara.KuaidiApp_Code;
                        ExpressData = new AliYiYuanService().ConvertToExpressData(new AliYiYuanService().Service(Para));
                        #endregion
                    }
                    string jsoncontent = JsonConvert.SerializeObject(ExpressData.ExpressDataItems);
                    //写库 根据isNewItem值判断是新增还是修改
                    DataCMD.AddorUp_WuliuMess(ExpressData.number, ExpressData.type, ExpressData.Status, ExpressData.date, jsoncontent, ExpressData.ApiName,isNewItem);
                }
                //HttpRuntime缓存
                //List<CacheCode> list = HttpRuntime.Cache.Get("CacheCode") as List<CacheCode>;
                //HttpRuntime.Cache.Insert();
                //MemoryCache缓存
                theManagerCache.CacheService.Set(LogisticPara.logisticsCode, ExpressData, 60 * 20, 2);
            }
            ViewBag.ExpressData = theManagerCache.CacheService.Get<object>(LogisticPara.logisticsCode);
            if (IsDebug)
            {
                Normal.WritLog("接受到的参数：" + JsonConvert.SerializeObject(ExpressData));
            }
            return View();
        }

    }

}
