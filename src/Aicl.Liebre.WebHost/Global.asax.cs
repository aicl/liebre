using System;
using ServiceStack.MiniProfiler;

namespace Aicl.Liebre.WebHost
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start()
		{

			var appHost= new AppHost();
			appHost.Init();

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