using Sitecore.Configuration;
using Sitecore.Web;
using Sitecore.Mvc.Pipelines.Request.RequestBegin;
using System.Web;
using Sitecore.Sites;

namespace Sitecore.Support.MVC.Pipelines.Request.RequestBegin
{
    public class CheckSaveRawUrlOnRedirection : RequestBeginProcessor
    {
        public override void Process(RequestBeginArgs args)
        {
            if (Settings.Authentication.SaveRawUrl
                && Sitecore.Context.User.Name == "sitecore\\Anonymous")

            {
                string somet = args.PageContext.RequestContext.HttpContext.Items["SC_REQUEST_MEASUREMENT_URL"].ToString();
                SiteContext site = Context.Site;
                string loginPage = this.GetLoginPage(site);
                string url = WebUtil.AddQueryString(loginPage + "?returnurl=" + HttpUtility.UrlEncode(somet));
                System.Web.HttpContext.Current.Response.Redirect(url);
            }
        }

        protected virtual string GetLoginPage(SiteContext site)
        {
            if (site != null)
            {
                if (site.DisplayMode == DisplayMode.Normal)
                {
                    return site.LoginPage;
                }
                SiteContext context = SiteContext.GetSite("shell");
                if (context != null)
                {
                    return context.LoginPage;
                }
            }
            return string.Empty;
        }
    }
}