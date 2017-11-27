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
{/// <summary>
    /// 说明：系统页面类
    /// 创建时间：2014-07-10
    /// 姓名：王晓苗
    /// </summary>
  public  class BizPage:BALBase
  {   
      /// <summary>
      /// 获得这个系统的所有页面
      /// </summary>
      /// <returns></returns>
      public static List <tb_Pages> GetAllPages()
      {
          return DataContext.tb_Pages.ToList();
      }  
      
      /// <summary>
      /// 根据PageID得到某个页面
      /// </summary>
      /// <param name="pageID"></param>
      /// <returns></returns>
      public static tb_Pages GetPageByID(long pageID)
      {
          return DataContext.tb_Pages .Where (t=>t.PageID ==pageID).SingleOrDefault();
      }

      /// <summary>
      /// 根据PageTitle得到某个页面
      /// </summary>
      /// <param name="pageTitle"></param>
      /// <returns></returns>
      public static tb_Pages GetPageByTitle(string pageTitle)
      {
          return DataContext.tb_Pages.Where(t => t.PageTitle == pageTitle).SingleOrDefault();
      }
     
      /// <summary>
      /// 添加新的页面
      /// </summary>
      /// <param name="pageTitle">页面名称</param>
      /// <param name="memo">备注</param>
      public static void AddNewPage(string pageTitle,string memo)
      {
          try
          {
              tb_Pages newPage = new tb_Pages();
              newPage.PageTitle = pageTitle;
              newPage.Memo = memo;
              DataContext.tb_Pages.AddObject(newPage);
              DataContext.SaveChanges();
          }
          catch (Exception ex)
          {
              LogHelper.g_Logger.ErrorFormat("添加页面信息失败！错误信息：{0}", ex.Message);
              throw ex;
          }
      }

      /// <summary>
      /// 根据PageID删除某个页面
      /// </summary>
      /// <param name="pageID"></param>
      public static void DeletePageByID(long pageID)
      {
          try
          {
              tb_Pages page = GetPageByID(pageID);
              DataContext.tb_Pages.DeleteObject(page);
              DataContext.SaveChanges();
          }
          catch (Exception ex)
          {
              LogHelper.g_Logger.ErrorFormat("删除页面信息失败！错误信息：{0}", ex.Message);
              throw ex;
          }
      }
      
      /// <summary>
      /// 根据PageID修改某个页面
      /// </summary>
      /// <param name="pageID"></param>
      /// <param name="pageTitle"></param>
      /// <param name="memo"></param>
      public static void UpdatePageByID(long pageID, string pageTitle, string memo)
      {
          try
          {
              tb_Pages page = GetPageByID(pageID);
              page.PageTitle = pageTitle;
              page.Memo = memo;
              DataContext.SaveChanges();
          }
          catch (Exception ex)
          {
              LogHelper.g_Logger.ErrorFormat("修改页面信息失败！错误信息：{0}", ex.Message);
              throw ex;
          }
      }
     
     /// <summary>
     /// 获得所有可以访问该页面的角色
     /// </summary>
     /// <param name="pageID"></param>
     /// <returns></returns>
      public static List<tb_Roles> GetPageRoles(long pageID)
      {
          var permissions = GetPageByID(pageID).tb_Permissions;
              List<tb_Roles> roles = new List<tb_Roles>();
          foreach (tb_Permissions per in permissions)
          {
              if (per.tb_Roles!= null)
                  roles.Add(per.tb_Roles);
          } 
          return roles;
      }
      
      /// <summary>
      /// 删除一个或多个角色访问该页面的权限
      /// </summary>
      /// <param name="pageID"></param>
      /// <param name="roleIDList"></param>
      public static void RemovePageRoles(long pageID, List<long> roleIDList)
      {
          try
          {
              tb_Pages page = GetPageByID(pageID);
              foreach (long id in roleIDList)
              {
                  tb_Permissions p = page.tb_Permissions.Where(t => t.RoleID == id).First();
                  page.tb_Permissions.Remove(p);
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
      /// 授予一个或多个角色访问该页面的权限
      /// </summary>
      /// <param name="pageID"></param>
      /// <returns></returns>
      public static void AddRolePages(long pageID, List<long> roleIDList)
      {
          try
          {
              tb_Pages page = GetPageByID(pageID);
              foreach (long id in roleIDList)
              {
                  tb_Permissions p = new tb_Permissions();
                  p.RoleID = id;
                  page.tb_Permissions.Add(p);
              }
              DataContext.SaveChanges();
          }
          catch (Exception ex)
          {
              LogHelper.g_Logger.ErrorFormat("授予角色权限失败！错误信息：{0}", ex.Message);
              throw ex;
          }
      }

      /// <summary>
      /// 获得所有可以访问该页面的用户
      /// </summary>
      /// <param name="pageID"></param>
      /// <returns></returns>
      public static List<tb_Mem_Member> GetPageMembers(long pageID)
      {
          var permissions = GetPageByID(pageID).tb_Permissions;
          List<tb_Mem_Member> members = new List<tb_Mem_Member>();
          foreach (tb_Permissions per in permissions)
          {   if(per.tb_Mem_Member!=null)
                members.Add(per.tb_Mem_Member);
          } 
          return members;
      }

      /// <summary>
      /// 删除一个或多个用户访问该页面的权限
      /// </summary>
      /// <param name="pageID"></param>
      /// <param name="roleIDList"></param>
      public static void RemovePageMembers(long pageID, List<long> memberIDList)
      {
          try
          {
              tb_Pages page = GetPageByID(pageID);
              foreach (long id in memberIDList)
              {
                  tb_Permissions p = page.tb_Permissions.Where(t => t.MemberID == id).First();
                  page.tb_Permissions.Remove(p);
                  DataContext.tb_Permissions.DeleteObject(p);
              }
              DataContext.SaveChanges();
          }
          catch (Exception ex)
          {
              LogHelper.g_Logger.ErrorFormat("删除用户权限失败！错误信息：{0}", ex.Message);
              throw ex;
          }
      }

      /// <summary>
      /// 授予一个或多个用户访问该页面的权限
      /// </summary>
      /// <param name="pageID"></param>
      /// <returns></returns>
      public static void AddMemberPages(long pageID, List<long> memberIDList)
      {
          try
          {
              tb_Pages page = GetPageByID(pageID);
              foreach (long id in memberIDList)
              {
                  tb_Permissions p = new tb_Permissions();
                  p.MemberID = id;
                  page.tb_Permissions.Add(p);
              }
              DataContext.SaveChanges();
          }
          catch (Exception ex)
          {
              LogHelper.g_Logger.ErrorFormat("授予用户权限失败！错误信息：{0}", ex.Message);
              throw ex;
          }
      }
  }
}
