using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logistics.Entity
{
    /// <summary>
    /// 商城物流公共返回对象
    /// </summary>
    public class ExpressData
    {
        public ExpressData()
        {
            ExpressDataItems = new ExpressDataItem[0];
        }

        /// <summary>
        /// 是否查询数据成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string number { get; set; }
        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 投递状态 1.在途中 2.正在派件 3.已签收 4.派送失败
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime date{ get; set; }
        /// <summary>
        /// 快递查询返回消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 快递公司代码
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// 快递物流详细信息（仅Success为True时有效）
        /// </summary>
        public IEnumerable<ExpressDataItem> ExpressDataItems { get; set; }
    }
    /// <summary>
    /// 快递实时数据
    /// </summary>
    public class ExpressDataItem
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
