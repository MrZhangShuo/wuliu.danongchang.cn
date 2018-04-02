using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logistics.Entity
{
   public class LogisticPara
    {
       /// <summary>
       /// 快递分类：2阿里易源
       /// </summary>
       public int Kuaidi_Type { get; set; }
       /// <summary>
       /// App_key
       /// </summary>
       public string KuaidiApp_key { get; set; }
       /// <summary>
       /// App_Secret
       /// </summary>
       public string KuaidiApp_Secret { get; set; }
       /// <summary>
       /// App_Code
       /// </summary>
       public string KuaidiApp_Code { get; set; }
       /// <summary>
       /// 快递公司代码
       /// </summary>
       public string shipperCode { get; set; }
       /// <summary>
       /// 物流单号
       /// </summary>

       public string logisticsCode { get; set; }


    }
}
