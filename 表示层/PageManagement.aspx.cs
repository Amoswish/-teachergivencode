using BAL.FurniturePay.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.FurniturePay.Helpers;
using BAL.FurniturePay.Business.Admin;
using BAL.FurniturePay.Bases;
using DAL.FurniturePay.Model;

namespace WebSite.FurniturePay.Admin
{
    public partial class PageManagement : MyAdminPage 
    {
        private void FreshData(int index)
        {
            this.aspNetPager.RecordCount = BizPage.GetAllPages().Count;
            pageGridView.DataSource = BizPage.GetAllPages().Skip(index * 10).Take(10).ToList();
            DataBind();   
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FreshData(0);
            }
        }

        protected void aspNetPager_PageChanged(object sender, EventArgs e)
        {
            FreshData(this.aspNetPager.CurrentPageIndex - 1);
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            BizPage.AddNewPage(txtPageTitle.Text, txtMemo.Text);
            txtPageTitle.Text = "";
            txtMemo.Text = "";
            int count = BizPage.GetAllPages().Count;
            this.aspNetPager.RecordCount = count;
            this.aspNetPager.CurrentPageIndex = count / 10 + 1;
            FreshData(this.aspNetPager.CurrentPageIndex - 1);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "myscript", "$('#showBox').html('成功添加一个新页面!');", true);
            this.ModalPopupExtender2.Show();
        }

        protected void pageGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            pageGridView.EditIndex = -1;
            FreshData(this.aspNetPager.CurrentPageIndex - 1);
        }

        protected void pageGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //long idx = (long)e.Keys["PageID"];
            //BizPage.DeletePageByID(idx);
            //int count = BizPage.GetAllPages().Count;
            //this.aspNetPager.RecordCount = count;
            //this.aspNetPager.CurrentPageIndex = count / 10 + 1;
            //FreshData(this.aspNetPager.CurrentPageIndex - 1);
        }

        protected void pageGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            pageGridView.EditIndex = e.NewEditIndex;
            FreshData(this.aspNetPager.CurrentPageIndex - 1);
        }

        protected void pageGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            long idx = long.Parse(e.Keys[0].ToString());
            BizPage.UpdatePageByID(idx, e.NewValues["PageTitle"].ToString(), e.NewValues["Memo"].ToString());
            pageGridView.EditIndex = -1;
            FreshData(this.aspNetPager.CurrentPageIndex - 1);
        }

        protected void lbPageAuthoritySettings_Click(object sender, EventArgs e)
        {
            authoritySettingPageID = long.Parse((sender as LinkButton).CommandArgument);
        }
       
        long authoritySettingPageID = -1;
        public long AuthoritySettingPageID { get { return authoritySettingPageID; } }

        protected void vldExistedPage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var res = BizPage.GetPageByTitle(args.Value.Trim().Replace(" ", ""));
            if (res != null)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
    
}
