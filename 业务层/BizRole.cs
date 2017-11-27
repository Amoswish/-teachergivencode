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
namespace BAL.FurniturePay.Business.Admin
{   /// <summary>
    /// 说明：系统角色类
    /// 创建时间：2014-6-28
    /// 姓名：王晓苗
    /// </summary>
 public class BizRole:BALBase
 {   
     /// <summary>
     /// 获取系统内置的所有角色
     /// </summary>
     /// <param name="roleID"></param>
     /// <returns></returns>
     public static List<tb_Roles> GetAllRoles()
     {
         return DataContext.tb_Roles.ToList();
     }
     
     /// <summary>
     /// 根据角色ID获得角色
     /// </summary>
     /// <param name="roleID"></param>
     /// <returns></returns>
     public static tb_Roles GetRoleByID(long roleID)
     {
         return DataContext.tb_Roles.Where(t => t.RoleID == roleID).FirstOrDefault();
     }

     /// <summary>
     /// 根据角色名称获取角色
     /// </summary>
     /// <param name="roleName"></param>
     /// <returns></returns>
     public static tb_Roles GetRoleByName(string roleName)
     {
         return DataContext.tb_Roles.Where(t => t.RoleName == roleName).FirstOrDefault();
     }

     /// <summary>
     /// 添加新的角色
     /// </summary>
     /// <param name="roleName"角色名称></param>
     /// <param name="memo">角色备注</param>
     /// <returns></returns>
     public static tb_Roles AddRole(string roleName, string memo)
     {
         try
         {
             tb_Roles role = new tb_Roles();
             role.RoleName = roleName;
             role.Memo = memo;
             DataContext.tb_Roles.AddObject(role);
             DataContext.SaveChanges();
             return role;
         }
         catch (Exception ex)
         {
             LogHelper.g_Logger.ErrorFormat("添加角色失败！错误信息：{0}", ex.Message);
             throw ex;
         }
        
     }

     /// <summary>
     /// 根据角色ID删除角色
     /// </summary>
     /// <param name="roleID">角色ID</param>
     public static void DeleteRole(long roleID)
     {
             try
             {
                 tb_Roles role = DataContext.tb_Roles.Where(t => t.RoleID == roleID).FirstOrDefault();
                 DataContext.tb_Roles.DeleteObject(role);
                 DataContext.SaveChanges();
             }
             catch (Exception ex)
             {
                 LogHelper.g_Logger.ErrorFormat("删除角色失败！错误信息：{0}", ex.Message);
                 throw ex;
             }
     }
   
     /// <summary>
     /// 根据角色ID修改角色基本内容
    /// </summary>
    /// <param name="roleID"></param>
    /// <param name="roleName"></param>
    /// <param name="memo"></param>
     public static void UpdateRole(long roleID, string roleName, string memo)
     {
         try
         {
             tb_Roles role = GetRoleByID(roleID);
             role.RoleName = roleName;
             role.Memo = memo;
             DataContext.SaveChanges();
         }
         catch (Exception ex)
         {
             LogHelper.g_Logger.ErrorFormat("修改角色失败！错误信息：{0}", ex.Message);
             throw ex;
         }
     }

     /// <summary>
     /// 判断该角色中是否仍存在用户
     /// </summary>
     /// <param name="roleID"></param>
     /// <returns></returns>
     public static bool IsExistedMembersInRole(long roleID)
     {
         var memberRole = GetRoleByID(roleID).tb_MemberInRoles;
         if (memberRole.Count()>0)
         {
             return true;      
         }
         return false;
     }
     
     /// <summary>
     /// 获取某个角色可以访问的所有页面
     /// </summary>
     /// <param name="roleID"></param>
     /// <returns></returns>
     public static List<tb_Pages> GetRolePages(long roleID)
     {
         var permissions = GetRoleByID(roleID).tb_Permissions;
         List<tb_Pages> pages = new List<tb_Pages>();
         foreach (tb_Permissions per in permissions)
             pages.Add(per.tb_Pages);
         return pages;
     }

     /// <summary>
     /// 解除某个角色访问一个或多个页面的权限
     /// </summary>
     /// <param name="roleID"></param>
     /// <param name="pageIDList"></param>
     public static void RemoveRolePages(long roleID, List<long> pageIDList)
     {
         try
         {
             tb_Roles role = BizRole.GetRoleByID(roleID);
             foreach (long id in pageIDList)
             {
                 tb_Permissions p = role.tb_Permissions.Where(t => t.PageID == id).First();
                 role.tb_Permissions.Remove(p);
                 DataContext.tb_Permissions.DeleteObject(p);
             }
             DataContext.SaveChanges();
         }
         catch (Exception ex)
         {
             LogHelper.g_Logger.ErrorFormat("删除角色权限失败！错误信息：{0}", ex.Message);
             throw ex;
         }
     }

     /// <summary>
     /// 授予某个角色访问一个或多个页面的权限
     /// </summary>
     /// <param name="roleID"></param>
     /// <param name="pageIDList"></param>
     public static void AddRolePages(long roleID, List<long> pageIDList)
     {
         try
         {
             tb_Roles role = BizRole.GetRoleByID(roleID);
             foreach (long id in pageIDList)
             {
                 tb_Permissions p = new tb_Permissions();
                 p.PageID = id;
                 role.tb_Permissions.Add(p);
             }
             DataContext.SaveChanges();
         }
         catch (Exception ex)
         {
             LogHelper.g_Logger.ErrorFormat("授予角色权限失败！错误信息：{0}", ex.Message);
             throw ex;
         }
     }
 }
}
