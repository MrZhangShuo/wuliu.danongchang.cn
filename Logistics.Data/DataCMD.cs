using Logistics.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Logistics.Data
{
    public class DataCMD
    {
        /// <summary>
        /// 新增或更新单号物流信息
        /// </summary>
        /// <param name="expressNO">单号</param>
        /// <param name="expressCom">快递公司代码</param>
        /// <param name="deliveryStatus">投递状态 1.在途中 2.正在派件 3.已签收 4.派送失败</param>
        /// <param name="UpdateTime">最后更新时间</param>
        /// <param name="JSONcontent">第三方接口返回信息</param>
        /// <param name="ApiName">第三方接口代码</param>
        /// <param name="state">判断物流信息入库操作 true：插入 false：更新</param>
        /// <returns></returns>
        public static int AddorUp_WuliuMess(string expressNO, string expressCom, int deliveryStatus,DateTime UpdateTime,string JSONcontent, string ApiName,bool state)
        {
            string sql;
            if (state == true)
            {
                sql = "INSERT INTO TB_ExpressInfo(expressNO,expressCom,deliveryStatus,UpdateTime,JSONcontent,ApiName)VALUES  (@expressNO,@expressCom,@deliveryStatus,@UpdateTime,@JSONcontent,@ApiName)";
            }
            else
            {
                sql = "update TB_ExpressInfo set expressCom=@expressCom,deliveryStatus=@deliveryStatus,UpdateTime=@UpdateTime,JSONcontent=@JSONcontent,ApiName=@ApiName where expressNO=@expressNO";
            }
            SqlDBHelper HelperOther = new SqlDBHelper();
            SqlParameter[] para = new SqlParameter[]{
            new SqlParameter("@expressNO",SqlDbType.VarChar,200),
            new SqlParameter("@expressCom",SqlDbType.VarChar,200),
            new SqlParameter("@deliveryStatus",SqlDbType.Int),
            new SqlParameter("@UpdateTime",SqlDbType.DateTime),
            new SqlParameter("@JSONcontent",JSONcontent),
            new SqlParameter("@ApiName",SqlDbType.VarChar,200)
            };
            para[0].Value = expressNO;
            para[1].Value = expressCom;
            para[2].Value = deliveryStatus;
            para[3].Value = UpdateTime;
            para[4].Value = JSONcontent;
            para[5].Value = ApiName;
            int result = HelperOther.ExecuteSql(sql, para);
            return result;
        }
    }
}
