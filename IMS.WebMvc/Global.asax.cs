using IMS.Data;
using IMS.WebMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IMS.WebMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new IMS.WebMvc.Models.ProductionSeedData());
            Database.SetInitializer<ApplicationDbContext>(new IdentityDbInitializer());

            ApplicationDbContext context = new ApplicationDbContext();
            context.Database.Initialize(true);

            DataContext dataContext = new DataContext();
            dataContext.Database.Initialize(true);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MapConfig.RegisterMaps();
        }
    }
}
