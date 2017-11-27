using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using DAL.FurniturePay.Model;
using DAL.FurniturePay.Helpers;
using BAL.FurniturePay.Business.Admin;
using BAL.FurniturePay.Business;
namespace BAL.FurniturePay.Bases
{    /// <summary>
    /// 说明：页面基类，用于在每个页面加载时判断用户的权限
    /// 创建时间：2014-6-30
    /// 姓名：王晓苗
    /// </summary>
    /// 
    public class MyPage : System.Web.UI.Page
    {   /// <summary>
        ///  在每个页面加载时判断用户访问该页面的权限
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                tb_Mem_Member currentUser = BizMember.GetMemberByID(long.Parse(User.Identity.Name));
                tb_Pages currentPage = BizPage.GetPageByTitle(this.Title);
                if (!BALPermission.ValidateMemberPagePermission(currentUser, currentPage))
                {
                    Response.Redirect("~/Common/Aspx/ErrorPage.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}
