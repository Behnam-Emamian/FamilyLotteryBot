using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace FamilyLotteryBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        static readonly ILog Logger = LogManager.GetLogger("Errors");
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Error("", (Exception)e.ExceptionObject);
        }
    }
}
