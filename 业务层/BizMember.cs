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
using BAL.FurniturePay.Business.Admin;
using System.Text.RegularExpressions;

namespace BAL.FurniturePay.Business
{
    /// <summary>
    /// 说明：会员基类，包含了各个子会员类需要的公共方法
    /// 创建时间：2014-7-15
    /// 姓名：王晓苗
    /// </summary>
    public class BizMember : BALBase
    {
        /// <summary>
        /// 根据ID查询会员，并返回查询结果
        /// </summary>
        /// <param name="memberID">会员ID</param>
        /// <returns></returns>
        public static tb_Mem_Member GetMemberByID(long memberID)
        {
            return DataContext.tb_Mem_Member.Where(t => t.MemberID == memberID).SingleOrDefault();

        }

        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        public static List<tb_Mem_Member> GetAllMember()
        {
            return DataContext.tb_Mem_Member.ToList();
        }

        /// <summary>
        /// 对敏感数据进行哈希加密
        /// </summary>
        /// <param name="code">需要加密的字段</param>
        /// <returns></returns>
        public static string Hash(string code, long memberID)
        {
            tb_Mem_Member member = GetMemberByID(memberID);
            System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1Managed.Create();
            code = Convert.ToBase64String(hash.ComputeHash(System.Text.Encoding.Unicode.GetBytes(code + member.Salt)));
            return code;
        }

        /// <summary>
        /// 验证账户名和登录密码是否匹配
        /// </summary>
        /// <param name="memberAccountName">家易宝账户名</param>
        /// <param name="loginPW">登录密码</param>
        /// <returns></returns>
        public static bool ValidateLoginPW(string memberAccountName, string loginPW)
        {
            tb_Mem_Member member = DataContext.tb_Mem_Member.Where(t => t.MemberAccountName == memberAccountName).SingleOrDefault();
            if (member != null)
            {
                string code = Hash(loginPW, member.MemberID);
                return (code == member.MemLogPas) ? true : false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证账户名和登录密码是否匹配(webService)
        /// </summary>
        /// <param name="memberAccountName">家易宝账户名</param>
        /// <param name="loginPW">明文登录密码</param>
        ///  <param name="memberType">会员类别 0个人会员 1企业会员 2.系统管理员 可为空 为空不判断</param>
        /// <returns>-1失败， >0 成功（用户ID）-2:被冻结</returns>
        public long ValidateLoginPW_WebService(string memberAccountName, string loginPW, int? memberType)
        {
            tb_Mem_Member member = DataContext.tb_Mem_Member.Where(t => t.MemberAccountName == memberAccountName).SingleOrDefault();
            if (member != null)
            {
                string code = Hash(loginPW, member.MemberID);
                long memberId = (code == member.MemLogPas) ? member.MemberID : -1;
                if (memberType != null)
                {
                    if (member.MemberType != memberType)
                    {
                        return -1;
                    }

                }
                //判断账户是否为正常状态
                if (member.Status == Convert.ToInt16(MemberStatus.Normal))
                {
                    return memberId;
                  
                }
                else
                {
                    //非正常状态 
                    return -2;
                }
            }
            return -1;
        }

        /// <summary>
        /// 验证账户名和支付密码是否匹配
        /// </summary>
        /// <param name="memberAccountName">家易宝账户名</param>
        /// <param name="payPW">支付密码</param>
        /// <returns></returns>
        public static bool ValidatePayPW(string memberAccountName, string payPW)
        {
            tb_Mem_Member member = DataContext.tb_Mem_Member.Where(t => t.MemberAccountName.Equals(memberAccountName)).SingleOrDefault();
            string code = Hash(payPW, member.MemberID);
            return (code == member.PayPas) ? true : false;
        }

        /// <summary>
        /// 根据会员ID验证支付密码
        /// </summary>
        /// <param name="payPW">支付密码</param>
        /// <returns></returns>
        public static bool ValidatePayPW(long memberID, string payPW)
        {
            tb_Mem_Member member = BizMember.GetMemberByID(memberID);
            string code = Hash(payPW, member.MemberID);
            return (code == member.PayPas) ? true : false;
        }

        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="NewLoginPW">新的登录密码</param>
        public static void ChangeLoginPW(long memberID, string newLoginPW)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                member.MemLogPas = Hash(newLoginPW, memberID);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("修改登录密码失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="NewPayPW">新的支付密码</param>
        public static void ChangePayPW(long memberID, string newPayPW)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                member.PayPas = Hash(newPayPW, member.MemberID);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("修改支付密码失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 判断账户名是否已经存在
        /// </summary>
        /// <param name="memberAccountName">家易宝账户名</param>
        /// <returns></returns>
        public static tb_Mem_Member QueryMemberByAccountName(string memberAccountName)
        {
            return DataContext.tb_Mem_Member.Where(t => t.MemberAccountName == memberAccountName).SingleOrDefault();
        }


        public static short GetMemberType(long memberID)
        {
            return (short)BizMember.GetMemberByID(memberID).MemberType;
        }

        /// <summary>
        /// 生成手机验证码，并设置该验证码的有效期限
        /// </summary>
        /// <param name="icode"></param>
        /// <param name="time"></param>
        public static void AddRegIdentifyingCode(string icode, long memberID)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                member.ValidTime = DateTime.Now.AddMinutes(30);
                member.RegIdentifyingCode = icode;
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("获取手机验证码失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 清空会员的手机验证码
        /// </summary>
        /// <param name="icode"></param>
        /// <param name="time"></param>
        public static void ClearIdentifyingCode(long memberID)
        {
            tb_Mem_Member member = BizMember.GetMemberByID(memberID);
            member.RegIdentifyingCode = null;
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 管理员修改个人人会员状态
        /// </summary>
        /// <param name="status"></param>
        public static void UpdateByAdmin(long memberID, short status)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                member.Status = status;
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("修改会员状态失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 将会员的登录次数加1
        /// </summary>
        /// <param name="memberID"></param>
        public static void AddLoginCount(long memberID)
        {
            tb_Mem_Member member = GetMemberByID(memberID);
            member.LoginCount += 1;
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 上传会员头像
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="memberID"></param>
        public static void UploadPhoto(byte[] buffer, long memberID)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                member.Image = buffer;
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("上传头像失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取会员头像文件
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static byte[] GetMemberPhoto(long memberID)
        {
            return BizMember.GetMemberByID(memberID).Image;
        }

        /// <summary>
        /// 将会员的登录次数置为0
        /// </summary>
        /// <param name="memberID"></param>
        public static void UpdateLoginCountToZero(long memberID)
        {
            tb_Mem_Member member = GetMemberByID(memberID);
            member.LoginCount = 0;
            DataContext.SaveChanges();
        }

        public static void RememberFailedLoginTime(long memberID)
        {
            tb_Mem_Member member = GetMemberByID(memberID);
            member.AccountValidTime = DateTime.Now;
            DataContext.SaveChanges();
        }

        ///<summary>
        ///将验证码失效
        /// </summary>
        /// <param name="memberID"></param>
        public static void UpdateRegIdentifyToNull(long memberID)
        {
            tb_Mem_Member member = GetMemberByID(memberID);
            member.RegIdentifyingCode = null;
            DataContext.SaveChanges();
        }


        /// <summary>
        /// 冻结会员账号
        /// </summary>
        /// <param name="memberName"></param>
        public static void LockMember(string memberName)
        {
            try
            {
                tb_Mem_Member member = QueryMemberByAccountName(memberName);
                member.AccountValidTime = DateTime.Now.AddMinutes(15);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("冻结会员失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 获取该员工的所有角色
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static List<tb_Roles> GetMemberRoles(long memberID)
        {
            var roles = BizMember.GetMemberByID(memberID).tb_MemberInRoles;
            List<tb_Roles> bindroles = new List<tb_Roles>();
            foreach (tb_MemberInRoles r in roles)
                bindroles.Add(r.tb_Roles);
            return bindroles;
        }

        /// <summary>
        /// 获得该会员可以访问的所有页面
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static List<tb_Pages> GetMemberPages(long memberID)
        {
            var permissions = BizMember.GetMemberByID(memberID).tb_Permissions;
            List<tb_Pages> pages = new List<tb_Pages>();
            foreach (tb_Permissions per in permissions)
                pages.Add(per.tb_Pages);
            return pages;
        }

        /// <summary>
        /// 同时为个人会员分配多个角色
        /// </summary>
        /// <param name="memberID">会员ID</param>
        /// <param name="roleIDList">角色ID数组</param>
        public static void AddMemberToRoles(long memberID, List<long> roleIDList)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                foreach (long id in roleIDList)
                {
                    tb_MemberInRoles memberRole = new tb_MemberInRoles();
                    memberRole.RoleID = id;
                    member.tb_MemberInRoles.Add(memberRole);
                    BizRole.GetRoleByID(id).tb_MemberInRoles.Add(memberRole);
                }
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("授予会员角色失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 为个人会员分配系统默认的个人会员角色
        /// </summary>
        /// <param name="memberID">会员ID</param>
        public static void AddMemberToDefaultRole(long memberID)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                tb_MemberInRoles memberRole = new tb_MemberInRoles();
                memberRole.RoleID = (long)RoleType.PersonalMember;
                member.tb_MemberInRoles.Add(memberRole);
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("授予会员角色失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 同时解除个人会员的多个角色
        /// </summary>
        /// <param name="memberID">会员ID</param>
        /// <param name="roleIDList">角色ID数组</param>
        public static void RemoveMemberFromRoles(long memberID, List<long> roleIDList)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                foreach (long id in roleIDList)
                {
                    tb_MemberInRoles memberRole = member.tb_MemberInRoles.Where(t => t.RoleID == id).First();
                    member.tb_MemberInRoles.Remove(memberRole);
                    DataContext.tb_MemberInRoles.DeleteObject(memberRole);
                }
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("解除会员角色失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 解除会员访问某些页面的权限
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="pageIDList"></param>
        public static void RemoveMemberPages(long memberID, List<long> pageIDList)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                foreach (long id in pageIDList)
                {
                    tb_Permissions p = member.tb_Permissions.Where(t => t.PageID == id).First();
                    member.tb_Permissions.Remove(p);
                    DataContext.tb_Permissions.DeleteObject(p);
                }
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("解除会员权限失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 授予会员访问一个或多个页面的权限
        /// </summary>
        /// <param name="memberID"></param>
        /// <param name="pageIDList"></param>
        public static void AddMemberPages(long memberID, List<long> pageIDList)
        {
            try
            {
                tb_Mem_Member member = BizMember.GetMemberByID(memberID);
                foreach (long id in pageIDList)
                {
                    tb_Permissions p = new tb_Permissions();
                    p.PageID = id;
                    member.tb_Permissions.Add(p);
                }
                DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.g_Logger.ErrorFormat("授予会员权限失败！错误信息：{0}", ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 验证会员身份
        /// </summary>
        /// <param name="memberName"></param>
        public static int ValidateMember(string accountName, string passWord)
        {
            tb_Mem_Member member = QueryMemberByAccountName(accountName);
            if (member == null || member.MemberType == (short)MemberType.SysAdmin)//判断该会员账户是否存在
            {
                return (int)ErrorTypeWhenLogin.FalseAccount;
            }
            else
            {
                if (member.AccountValidTime > DateTime.Now || member.Status == (short)MemberStatus.Frozen)//判断该账户是否暂时被冻结
                {
                    return (int)ErrorTypeWhenLogin.LockedAccount;
                }
                else
                {
                    BizEnterpriseMember enterprise = new BizEnterpriseMember();
                    long tem = enterprise.GetRolesByAccountName(member.MemberID);
                    //距离上次登录失败15分钟后，将登录次数重置为0
                    if (member.AccountValidTime < DateTime.Now.AddMinutes(-15) && member.LoginCount != 0)
                    {
                        BizMember.UpdateLoginCountToZero(member.MemberID);
                    }
                    if (tem != (short)RoleType.UnCheckedEnterpriseMember && (member.Status != (short)MemberStatus.Normal))
                    {
                        return (int)ErrorTypeWhenLogin.UnActivatedAccount;
                    }
                    else
                    {
                        if (!ValidateLoginPW(accountName, passWord))//判断账户密码是否正确
                        {   //输错密码一次，将登录次数加1
                            BizMember.AddLoginCount(member.MemberID);
                            BizMember.RememberFailedLoginTime(member.MemberID);
                            if (member.LoginCount == 3)
                            {   //三次输错密码后冻结账户，并将登录次数重置为0
                                BizMember.LockMember(accountName);
                                BizMember.UpdateLoginCountToZero(member.MemberID);
                                return (int)ErrorTypeWhenLogin.ThreeTimesLogin;
                            }
                            return (int)ErrorTypeWhenLogin.FalsePassword;
                        }
                        else
                        {
                            BizMember.UpdateLoginCountToZero(member.MemberID);
                            return (int)ErrorTypeWhenLogin.SuccessfulLogin;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证管理员身份
        /// </summary>
        /// <param name="memberName"></param>
        public static int ValidateSysAdmin(string accountName, string passWord)
        {
            tb_Mem_Member member = QueryMemberByAccountName(accountName);
            if (member == null || member.MemberType != (short)MemberType.SysAdmin)//判断该管理员账户是否存在
            {
                return (int)ErrorTypeWhenLogin.FalseAccount;
            }
            else
            {
                if (member.Status == (short)MemberStatus.Disable)
                {
                    return (int)ErrorTypeWhenLogin.UnActivatedAccount;
                }
                else if (member.AccountValidTime > DateTime.Now)//判断该账户是否暂时被冻结
                {
                    return (int)ErrorTypeWhenLogin.LockedAccount;
                }
                else
                {   //距离上次登录失败15分钟后，将登录次数重置为0
                    if (member.AccountValidTime < DateTime.Now.AddMinutes(-15) && member.LoginCount != 0)
                    {
                        BizMember.UpdateLoginCountToZero(member.MemberID);
                    }
                    if (!ValidateLoginPW(accountName, passWord))//判断账户密码是否正确
                    {   //输错密码一次，将登录次数加1
                        BizMember.AddLoginCount(member.MemberID);
                        BizMember.RememberFailedLoginTime(member.MemberID);
                        if (member.LoginCount == 3)
                        {   //三次输错密码后冻结账户，并将登录次数重置为0
                            BizMember.LockMember(accountName);
                            BizMember.UpdateLoginCountToZero(member.MemberID);
                            return (int)ErrorTypeWhenLogin.ThreeTimesLogin;
                        }
                        return (int)ErrorTypeWhenLogin.FalsePassword;
                    }
                    else
                    {
                        BizMember.UpdateLoginCountToZero(member.MemberID);
                        return (int)ErrorTypeWhenLogin.SuccessfulLogin;
                    }
                }
            }
        }
        /// <summary>
        /// 修改密保问题
        /// </summary>
        /// <param name="memberID">会员ID</param>
        /// <param name="question">问题提示</param>
        /// <param name="answer">问题答案</param>
        public void AddSafeProblem(long memberID, string question, string answer)
        {
            tb_Mem_Member member = GetMemberByID(memberID);
            member.Question = question;
            member.Answer = answer;
            DataContext.SaveChanges();
        }
    }
}
