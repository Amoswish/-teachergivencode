<%@ Page Language="C#" Title="页面管理" AutoEventWireup="true" CodeBehind="PageManagement.aspx.cs" Inherits="WebSite.FurniturePay.Admin.PageManagement" MasterPageFile="~/Masters/DefaultMaster/M_SysAdminMaster.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
      <div id="content">
<div class="content cf">
  <div class="cont-box cf">
  <div class="content-head"><h1>页面管理</h1></div>
    <div class="cont-padding cf">
      <div class="systemadmin-head cf">


          <script type='text/javascript'>
              function cancelClick() {
                  var label = $get('ctl00_SampleContent_Label1');
              }
</script><div>
      <%-- 删除公告遮罩层開始部分 --%>
                    <asp:Panel ID="pnlDelPage" runat="server" Style="text-align: center; display: none;">
                        <div class="netbank" id="netbank" style="width: 314px; height: 160px; display: block;">
                            <div class="netbank-head"><span class="lt">删除页面</span> </div>
                            <div class="netbank-list">
                                <div style="font-size: 16px; text-align-last: center; height: 60px;">你确定要删除该页面吗？</div>
                                <div>
                                    <span style="padding-left: 5px">
                                        <asp:Button ID="btnYes" runat="server" Width="60" Text="是" CausesValidation="false"/>
                                    </span><span style="padding-right: 56px"></span><span style="padding-right: 5px">
                                        <asp:Button ID="btnNO" runat="server" Width="60" Text="否" CausesValidation="false" /></span>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <%--结束 --%>
     <ajax:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnBlock" DisplayModalPopupID="ModalPopupExtender2" />
     <ajax:ModalPopupExtender ID="ModalPopupExtender2" runat="server" BackgroundCssClass="modalBackground" TargetControlID="btnBlock" PopupControlID="pnlShow" CancelControlID="btnConfirm"/>
    <asp:Button ID="btnBlock" runat="server" Text="Button" style="display:none" />
    
     <asp:Panel ID="pnlShow" runat="server" style="display:none;" BorderWidth="1px" >  
          <style type="text/css">
    .modalBackground {
	background-color:Gray;
	filter:alpha(opacity=70);
	opacity:0.7;
}
</style>
       <div class="netbank" style="width:250px;height:150px">
          <div class="netbank-head"> <span class="lt">提示</span></div>
          <div class="netbank-list">
           <table width="100%">
               <tr>
                   <td style="font-size:15px; text-align:center" id="showBox"></td>
               </tr>
               <tr>
                   <td style="height:60px; text-align:center">
                       <asp:Button ID="btnConfirm" CssClass="btnbox" runat="server" Width="60" Text="确定" CausesValidation="false"/>
                   </td>
               </tr>
           </table>
          </div>
        </div>
     </asp:Panel>
    <asp:UpdatePanel ID="main" runat="server">
        <ContentTemplate>
           <div align="center">
             <asp:GridView ID="pageGridView" runat="server" AutoGenerateColumns="False" SkinId="MainSkin" RowStyle-HorizontalAlign="Left" style="width:100%" DataKeyNames="PageID" OnRowCancelingEdit="pageGridView_RowCancelingEdit" OnRowDeleting="pageGridView_RowDeleting" OnRowEditing="pageGridView_RowEditing" OnRowUpdating="pageGridView_RowUpdating">
                <Columns>
                    <asp:TemplateField HeaderText="页面名称">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" SkinId="MainSkin" Text='<%# Bind("PageTitle") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1" ErrorMessage="页面名称不可为空" ValidationGroup="1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("PageTitle") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="页面权限">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbPageAuthoritySettings" runat="server" CausesValidation="false" CommandName="grant" CommandArgument='<%# Bind("PageID") %>' Text="设置" PostBackUrl="PageAuthoritySetting.aspx" OnClick="lbPageAuthoritySettings_Click"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="100px"/>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ValidationGroup="1">
                        <ItemStyle Width="100px"/>
                    </asp:CommandField>
                    <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                           <ajax:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="btnDel" OnClientCancel="cancelClick" DisplayModalPopupID="ModalPopupExtender1" />
                                        <ajax:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnDel" PopupControlID="pnlDelPage" OkControlID="btnYes" CancelControlID="btnNO" BackgroundCssClass="modalBackground" />
                            <asp:LinkButton ID="btnDel" runat="server" CausesValidation="False" CommandName="Delete" Text="删除"></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="100px"/>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                 <webdiyer:AspNetPager ID="aspNetPager" runat="server" CssClass="page-a rt mr15 mb15" FirstPageText="第一页"
                                         HorizontalAlign="Right" LastPageText="最后一页" NextPageText="下一页" NumericButtonCount="5"
                                        OnPageChanged="aspNetPager_PageChanged" PageSize="10" PagingButtonSpacing="10px"
                                        PrevPageText="上一页" ShowNavigationToolTip="True"
                                        Width="100%" Wrap="False" CustomInfoHTML="" AlwaysShow="true">
                                        </webdiyer:AspNetPager>  
               </div>
            <hr />
             <table align="center" width="100%" style="margin:0px auto;">
                    <tr><td colspan="3" align="center" style="background-color: #5D7B9D; color: #FFFFFF; font-weight: bold;">新建页面</td></tr>
                </table>
       <table align="center" style="margin:0px auto;">
                   <tr>
                    <td style="height: 32px;width: 176px" align="right">页面名称:&nbsp;&nbsp;&nbsp;&nbsp;
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtPageTitle" runat="server" onblur="return checkConfirm(this.value)"></asp:TextBox><span id="errinfo" class="bankcard"  style= "color:#FF0000;"></span>
                        <asp:RequiredFieldValidator ID="vldEmptyPageName" runat="server" ControlToValidate="txtPageTitle" ErrorMessage="页面名称不可为空!" ForeColor="Red"/>
                    <asp:CustomValidator ID="vldExistedPage" runat="server" ControlToValidate="txtPageTitle" ErrorMessage="该角色名已经存在！" Display="Dynamic" ForeColor="Red" OnServerValidate="vldExistedPage_ServerValidate"></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2" style="height: 32px;width: 176px" align="right">备注:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </td>
                    <td class="auto-style2">
                        <asp:TextBox ID="txtMemo" runat="server"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                      <td>
                          </td>
                      <td align="left">
                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnOK" runat="server" Text="添加" Width="60px" OnClick="btnOK_Click"/>
                       </td>
                </tr>
        </table>
        </ContentTemplate>     
    </asp:UpdatePanel>
    </div>
          </div>
  </div>
</div>
        </div>
        </div>

     <script type='text/javascript'>
         function checkConfirm(pageName) {
             var changeUrl = "/Common/Ashx/PageName.ashx?pageName=" + pageName;
             $.get(changeUrl, function (str) {

                 if (str == '1') {
                     $("#errinfo").html("该页面名称已经存在!");
                     $("#ContentPlaceHolder1_vldExistedPage").hide();

                 } else {
                     $("#errinfo").html("");
                     $("#ContentPlaceHolder1_vldExistedPage").hide();
                 }
             })
             $("#ContentPlaceHolder1_vldEmptyPageName").hide();
             return false;
         }
</script>
    </asp:Content>