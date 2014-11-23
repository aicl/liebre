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
				var result = hello.Get(new DiagnosticoInfo{Id=id} );
				System.Console.WriteLine (result.Guias [1].Respuesta.Valor.GetType());
				System.Console.WriteLine (result.Guias [1].Respuesta.Valor);
				return View (result);
			}
		}
	}
}