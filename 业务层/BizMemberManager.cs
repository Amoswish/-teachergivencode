using BAL.FurniturePay.Bases;
using DAL.FurniturePay.Helpers;
using DAL.FurniturePay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.FurniturePay.Business.Admin
{
    /// <summary>
    /// 说明：会员管理（注册会员表管理）
    /// 创建时间：2014-06-20
    /// 创建人：邓国林
    /// </summary>
    public class BizMemberManager : BALBase
    {
        private tb_Mem_Member member;
        /// <summary>
        /// 根据id返回序列中的元素
        /// </summary>
        /// <param name="id">会员ID</param>
        public BizMemberManager(long id)
        {
            member = DataContext.tb_Mem_Member.Where(t => t.MemberID == id).SingleOrDefault();

        }
        /// <summary>
        /// 无参构造函数的调用
        /// </summary>
        public BizMemberManager()
        {
        }
        /// <summary>
        /// 冻结账号
        /// </summary>
        /// <param name="freezeReason">审批备注</param>
        public void FreezeMember(long verifyID, string freezeReason)
        {
            if (member.Status == (short)MemberStatus.Normal)
            {
                member.ApproveDate = DateTime.Now;
                member.VerifyID = verifyID;
                member.ApproveRemark = freezeReason;
                member.Status = (short)MemberStatus.Frozen;
                DataContext.SaveChanges();
            }
        }

        /// <summary>
        /// 停用账号
        /// </summary>
        /// <param name="stopReason">审批备注</param>
        public void StopMember(long verifyID, string stopReason)
        {
            if (member.Status == (short)MemberStatus.Normal)
            {
                member.ApproveDate = DateTime.Now;
                member.VerifyID = verifyID;
                member.ApproveRemark = stopReason;
                member.Status = (short)MemberStatus.Disable;
                DataContext.SaveChanges();
            }
        }

        /// <summary>
        /// 解除冻结的账户
        /// </summary>
        public void ClearFreezeMember(long verifyID)
        {
            if (member.Status == (short)MemberStatus.Frozen)
            {
                member.VerifyID = verifyID;
                member.ApproveRemark = null;
                member.ApproveDate = DateTime.Now;
                member.Status = (short)MemberStatus.Normal;
            }
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 解除停用的账户
        /// </summary>
        public void ClearStopMember(long verifyID)
        {
            if (member.Status == (short)MemberStatus.Disable)
            {
                member.VerifyID = verifyID;
                member.ApproveRemark =null;
                member.ApproveDate = DateTime.Now;
                member.Status = (short)MemberStatus.Normal;
            }
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 给待审批的企业审批通过
        /// </summary>
        public void EnterpriseApplyGetThought(long id,long verifyID)
        {
            if (member.Status == (short)MemberStatus.ToExamine &&BizEnterprise.GetAllMemberInRoles(id).RoleID == (short)RoleType.UnCheckedEnterpriseMember)
            {
                member.Status = (short)MemberStatus.Normal;
                member.VerifyID = verifyID;
                member.ApproveRemark = null;
                BizEnterprise.GetAllMemberInRoles(id).RoleID =(short)RoleType.CheckedEnterpriseMember;       
                member.ApproveDate = DateTime.Now;
                //member.ApproveRemark = "通过申请";
            }
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 给待审批的企业拒绝通过
        /// </summary>
        /// <param name="rejectReason">拒绝原因</param>
        public void EnterpriseApplyRefuse(long verifyID, string rejectReason)
        {
            if (member.Status == (short)MemberStatus.ToExamine)
            {
                member.Status = (short)MemberStatus.NotThrough;
                member.VerifyID = verifyID;
                member.ApproveDate = DateTime.Now;
                member.ApproveRemark = rejectReason;
            }
            DataContext.SaveChanges();
        }
        /// <summary>
        /// 修改企业会员关联的家易择配商城账户
        /// </summary>
        /// <param name="id">企业ID</param>
        /// <param name="accountName">家易择配商城账户名</param>
        public void ChangAccountName(long id, string accountName)
        {
            tb_Mem_Member freeze = DataContext.tb_Mem_Member.Where(t => t.MemberID == id).SingleOrDefault();
            freeze.MallMemName = accountName;
            DataContext.SaveChanges();

        }
        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <param name="id">企业会员ID</param>
        /// <param name="telephone">手机号码</param>
        public void ChangeTelephoneNo(long id, string telephone)
        {
            tb_Mem_Member freeze = DataContext.tb_Mem_Member.Where(t => t.MemberID == id).SingleOrDefault();
            freeze.MobilePhone = telephone;
            DataContext.SaveChanges();
        }
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <param name="newpw">新密码</param>
        public void ChangPw(long id, string newpw)
        {
            member = BizMember.GetMemberByID(id);
            member.MemLogPas = BizMember.Hash(newpw, id);
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 获取会员头像文件
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        public static byte[] GetMemberPhoto(long memberID)
        {
            return DataContext.tb_Mem_Member.Where(t => t.MemberID == memberID).SingleOrDefault().Image;

        }
        /// <summary>
        /// 修改个人会员信息
        /// </summary>
        /// <param name="trueName">真实姓名</param>
        /// <param name="personalPhoneNo">手机号码</param>
        /// <param name="eMail">电子邮箱</param>
        /// <param name="identityCard">身份证号</param>
        /// <param name="sex">性别</param>
        public void EditPersonalMember(string trueName, string personalPhoneNo, string eMail, string identityCard, short? sex)
        {
            member.tb_Mem_PersonalInf.TrueName = trueName;
            member.tb_Mem_PersonalInf.PersonalPhoneNo = personalPhoneNo;
            member.EMail = eMail;
            member.tb_Mem_PersonalInf.IdentityCard = identityCard;
            member.tb_Mem_PersonalInf.Sex = sex;
            DataContext.SaveChanges();
        }
       /// <summary>
       /// 修改企业会员信息
       /// </summary>
       /// <param name="companyName">公司名称</param>
        /// <param name="businesslicenseNo">营业执照注册号</param>
        /// <param name="businesslicenseArea">LicenseAddress</param>
        /// <param name="personalPhoneNo">联系电话</param>
        /// <param name="address">Address</param>
        /// <param name="organizationNo">组织机构代码</param>
        /// <param name="registeredCapital">注册资金</param>
       /// <param name="fax">传真</param>
       /// <param name="legalName">法人真实姓名</param>
       /// <param name="legalPhone">法人手机号码</param>
       /// <param name="eMail">电子邮箱</param>
        public void EditEnterpriseMember(string companyName, string businesslicenseNo, string businesslicenseArea, string personalPhoneNo, string address, string organizationNo, float? registeredCapital, string fax, string legalName, string legalPhone, string eMail, DateTime? BusinesslicenseDate)
        {
            member.tb_Mem_EnterpriseInf.CompanyName = companyName;
            member.tb_Mem_EnterpriseInf.LicenseNo = businesslicenseNo;
            member.tb_Mem_EnterpriseInf.LicenseAddress = businesslicenseArea;
            member.tb_Mem_EnterpriseInf.Telephone = personalPhoneNo;
            member.tb_Mem_EnterpriseInf.Address = address;
            member.tb_Mem_EnterpriseInf.OrganizationCode = organizationNo;
            member.tb_Mem_EnterpriseInf.Capital = registeredCapital;
            member.tb_Mem_EnterpriseInf.Fax = fax;
            member.tb_Mem_EnterpriseInf.LegalName = legalName;
            member.tb_Mem_EnterpriseInf.LegalPhone = legalPhone;
            member.tb_Mem_EnterpriseInf.ToDate = BusinesslicenseDate;
            member.EMail = eMail;
            DataContext.SaveChanges();
        }
    }


}
