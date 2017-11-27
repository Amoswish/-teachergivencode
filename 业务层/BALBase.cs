using BAL.FurniturePay.Helpers;
using DAL.FurniturePay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Data.Objects;
using System.Data;

namespace BAL.FurniturePay.Bases
{    /// <summary>
    /// 说明：基类
    /// 创建时间：2014-6-13
    /// 姓名：王晓苗
    /// </summary>
    public class BALBase
    {
        //private static FurniturePayDBEntities dataContext;
        //static BALBase()
        //{
        //    dataContext = ((FurniturePayDBEntities)HttpContext.Current.Items["FurniturePayDBEntities"]);
        //}
        public static FurniturePayDBEntities DataContext
        {
            get
            {
                FurniturePayDBEntities dataContext = (FurniturePayDBEntities)HttpContext.Current.Items["FurniturePayDBEntities"];
                if (dataContext == null)
                {
                    throw new ArgumentNullException("数据上下文不存在！");
                }
                return dataContext;
            }
        }
    }
}
