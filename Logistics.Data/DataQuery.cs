using Logistics.Common;
using Logistics.Entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Logistics.Data
{
    public class DataQuery
    {
        /// <summary>
        /// 根据单号取物流信息
        /// </summary>
        /// <param name="expressNO"></param>
        /// <returns></returns>
        public static ExpressData Query_WuliuMess(string expressNO)
        {
            ExpressData express_mess = null;
            SqlDBHelper HelperOther = new SqlDBHelper();
            string sql = "SELECT * FROM [dbo].[TB_ExpressInfo] where expressNO=@expressNO";
            SqlParameter[] para = new SqlParameter[]{
            new SqlParameter("@expressNO",SqlDbType.VarChar,200),
            };
            para[0].Value = expressNO;
            DataTable dt = HelperOther.Query(sql, para).Tables[0];
            foreach (DataRow ro in dt.Rows)
            {
                //string ExpressContent = ro["JSONcontent"].ToString();
                List<ExpressDataItem> content = JsonHelper.DeserializeJsonToList<ExpressDataItem>(ro["JSONcontent"].ToString());
                express_mess = new ExpressData { Success = ro["deliveryStatus"].ToString() == "0" ? false : true, number = ro["expressNO"].ToString(), type = ro["expressCom"].ToString(), Status = int.Parse(ro["deliveryStatus"].ToString()), date = Convert.ToDateTime(ro["UpdateTime"]), ExpressDataItems = content, Message = "物流获取成功", ApiName = ro["ApiName"].ToString()};
            }

            return express_mess;
        }
    }
}
