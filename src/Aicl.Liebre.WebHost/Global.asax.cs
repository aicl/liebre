using System;
using System.Web.Mvc;
using System.Web.Routing;
using ServiceStack.MiniProfiler;

namespace Aicl.Liebre.WebHost
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("Content/{*pathInfo}");
			routes.IgnoreRoute("lbr-api/{*pathInfo}"); 
			routes.IgnoreRoute("");
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

			routes.MapRoute("CatchAll", "{*url}",
				new { controller = "Home", action = "Index" }
			);
		}

		protected void Application_Start()
		{
			ViewEngines.Engines.Clear ();
			ViewEngines.Engines.Add (new RazorViewEngine ());
			AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			var appHost= new AppHost();
			appHost.Init();
			RegisterRoutes(RouteTable.Routes);
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (Request.IsLocal)
				Profiler.Start();
		}

		protected void Application_EndRequest(object sender, EventArgs e)
		{
			Profiler.Stop();
		}
	}
}