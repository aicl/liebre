using System;
using ServiceStack.Mvc;
using System.Web.Mvc;
using ServiceStack;
using Aicl.Liebre.ServiceInterface;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.WebHost
{
	public class DiagnosticoInfoController:ServiceStackController
	{
		public ActionResult Index ()
		{
			var id = Request.QueryString["Id"]??Request.QueryString["id"];
			using (var hello = HostContext.ResolveService<DiagnosticoInfoService>(HttpContext))
			{
				Console.WriteLine ("Id :'{0}'", id);
				var result = hello.Get(new DiagnosticoInfo{Id=id} );
				return View (result);
			}
		}
	}
}