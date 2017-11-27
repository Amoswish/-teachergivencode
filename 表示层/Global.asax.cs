using DAL.FurniturePay.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

// Load the configuration from the 'log4net.config' file
/*** style 1
//[assembly: log4net.Config.XmlConfigurator(ConfigFileExtension = "log4net", Watch = true)]
  style 1 end ***/
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

 namespace WebSite.FurniturePay
{
    public class Global : System.Web.HttpApplication
    {
        private static WebContextModule module = new WebContextModule();

        public override void Init()
        {
            base.Init();
            module.Init(this);
            
        }
        /////////////////////////////////////////////////////////

        void Application_Start(object sender, EventArgs e)
        {
            string path = Server.MapPath("~/log4net.config");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            LogHelper.g_Logger = log4net.LogManager.GetLogger("");

            Application["Count"] = 0; 
            //WebSite.FurniturePay.Web.ASPxClasses.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
        }

        void Application_End(object sender, EventArgs e)
        {
            // Code that runs on application shutdown
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            try
            {
              // Server.Transfer("~/Error.aspx");
            }
            catch
            {
                // ignore
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
             Application.Lock(); 
            //增加一个在线人数 
             Application["Count"] = (int)Application["Count"] + 1; 
           //解锁 
           Application.UnLock(); 
        }

        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            //减少一个在线人数 
            Application["Count"] = (int)Application["Count"] - 1;
            //解锁 
            Application.UnLock(); 
        }
    }
}