using BAL.FurniturePay.Helpers;
using DAL.FurniturePay.Model;
using BAL.FurniturePay.Bases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.FurniturePay.Helpers;

namespace BAL.FurniturePay.Bases
{
    /// <summary>
    /// 说明：用于判断用户页面权限的类
    /// 创建时间：2014-6-30
    /// 姓名：王晓苗
    /// </summary>
    public class BALPermission : BALBase
    {    /// <summary>
        /// 判断用户访问某个页面的权限
        /// </summary>
        /// <param name="memberID">会员ID</param>
        /// <returns></returns> 
        public static bool ValidateMemberPagePermission(tb_Mem_Member member, tb_Pages page)
        {
            if (member == null || page == null)
                return false;
            //先判断用户是否有权访问该页面
            tb_Permissions permission = DataContext.tb_Permissions.Where(t => t.MemberID == member.MemberID && t.PageID == page.PageID).SingleOrDefault();
            if (permission != null)
                return true;
            //若用户没有权限，判断他所属的角色是否有权访问该页面
            else
            {
                foreach (tb_MemberInRoles role in member.tb_MemberInRoles)
                {
                    permission = DataContext.tb_Permissions.Where(t => t.RoleID == role.RoleID && t.PageID == page.PageID).SingleOrDefault();
                    if (permission != null)
                        return true;
                }
            }
            return false;
        }
    }
}
