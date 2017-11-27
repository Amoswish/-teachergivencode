using BAL.FurniturePay.Helpers;
using DAL.FurniturePay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Data.Objects;
using System.Data;

namespace WebSite.FurniturePay
{
   public class WebContextModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items["FurniturePayDBEntities"] = new FurniturePayDBEntities();
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var ctx = ((FurniturePayDBEntities)HttpContext.Current.Items["FurniturePayDBEntities"]);
            if (ctx != null)
                ctx.Dispose();
        }
    }
}
