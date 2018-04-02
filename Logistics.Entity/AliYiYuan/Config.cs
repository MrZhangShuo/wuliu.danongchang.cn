using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logistics.Entity.AliYiYuan
{
    /// <summary>
    /// https://market.aliyun.com/products/57126001/cmapi010996.html?spm=5176.730005.0.0.dO4BoH#sku=yuncode499600008
    /// </summary>
  public  class Config
    {

      /// <summary>
      /// 接口参数
      /// </summary>
      public class GetPara {
          /// <summary>
          /// AppCode
          /// </summary>
          public string AppCode { get; set; }
          /// <summary>
          /// 快递公司代码
          /// </summary>
          public string com { get; set; }
          /// <summary>
          /// 快递单号
          /// </summary>
          public string nu { get; set; }

      
      }


      public class ResultObj
      {
          /// <summary>
          /// showapi平台返回码,0为成功,其他为失败
          /// </summary>
          public int showapi_res_code { get; set; }
          /// <summary>
          /// showapi平台返回的错误信息
          /// </summary>
          public string showapi_res_error { get; set; }
          /// <summary>
          /// 物流信息
          /// </summary>
          public showapi_res_body showapi_res_body { get; set; }

      }

      public class showapi_res_body
      {
          /// <summary>
          /// 快递名称
          /// </summary>
          public string expTextName { get; set; }
          /// <summary>
          /// 快递配送信息
          /// </summary>
          public List<showapi_res_Item> data { get; set; }
          /// <summary>
          /// 物流单号
          /// </summary>
          public string mailNo { get; set; }
          /// <summary>
          /// 物流公司简称
          /// </summary>
          public string expSpellName { get; set; }
          /// <summary>
          /// 消息
          /// </summary>
          public string msg { get; set; }
          /// <summary>
          /// 更新时间
          /// </summary>
          public string updateStr { get; set; }
          /// <summary>
          /// 返回状态码：0成功
          /// </summary>
          public int ret_code { get; set; }
          /// <summary>
          /// 物流信息是否获取成功
          /// </summary>
          public bool flag { get; set; }
          /// <summary>
          /// -1 待查询 0 查询异常 1 暂无记录 2 在途中 3 派送中 4 已签收 5 用户拒签 6 疑难件 7 无效单 8 超时单 9 签收失败 10 退回
          /// </summary>
          public int status { get; set; }
          /// <summary>
          /// 数据记录数
          /// </summary>
          public int dataSize { get; set; }
          /// <summary>
          /// 数据最后查询的时间
          /// </summary>
          public long update { get; set; }
      }

      /// <summary>
      /// 物流信息
      /// </summary>
      public class showapi_res_Item
      {

          public string time { get; set; }
          public string context { get; set; }
      }
     

    }
}
