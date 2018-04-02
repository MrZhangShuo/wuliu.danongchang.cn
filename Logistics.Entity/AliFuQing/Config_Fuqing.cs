using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logistics.Entity.AliFuQing
{
    public class Config_Fuqing
    {
        /// <summary>
        /// 接口参数
        /// </summary>
        public class GetPara
        {
            /// <summary>
            /// AppCode
            /// </summary>
            public string AppCode { get; set; }
            /// <summary>
            /// 快递公司代码
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 快递单号
            /// </summary>
            public string no { get; set; }


        }


        public class ResultObj
        {
            /// <summary>
            /// showapi平台返回码,0为成功,其他为失败
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// showapi平台返回的错误信息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 物流信息
            /// </summary>
            public result result { get; set; }

        }

        public class result
        {
            /// <summary>
            /// 快递名称
            /// </summary>
            public string expName { get; set; }
            /// <summary>
            /// 快递配送信息
            /// </summary>
            public List<result_Item> list { get; set; }
            /// <summary>
            /// 物流单号
            /// </summary>
            public string number { get; set; }
            /// <summary>
            /// 物流公司简称
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 消息
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 物流信息是否获取成功
            /// </summary>
            public bool flag { get; set; }
            /// <summary>
            /// 1.在途中 2.正在派件 3.已签收 4.派送失败 
            /// </summary>
            public int deliverystatus { get; set; }
            /// <summary>
            /// 是否签收
            /// </summary>
            public int issign { get; set; }
        }

        /// <summary>
        /// 物流信息
        /// </summary>
        public class result_Item
        {

            public string time { get; set; }
            public string status { get; set; }
        }
    }
}
