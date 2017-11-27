using BAL.FurniturePay.Bases;
using BAL.FurniturePay.Business;
using BAL.FurniturePay.Business.Admin;
using DAL.FurniturePay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL.FurniturePay.Bases
{
    public class MyAdminPage  : System.Web.UI.Page
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
                Response.Redirect("~/Admin/AdminLogin.aspx");
            }
        }
    }
}