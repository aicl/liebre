/*
using System;
using ServiceStack.Mvc;
using System.Web.Mvc;
using ServiceStack;
using Aicl.Liebre.ServiceInterface;
using Aicl.Liebre.Model;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Aicl.Liebre.WebHost
{

	public class DiagnosticoInfoController:ServiceStackController
	{
		public ActionResult Index ()
		{
			var id = Request.QueryString["Id"]??Request.QueryString["id"];
			using (var hello = HostContext.ResolveService<DiagnosticoInfoService>(HttpContext))
			{
				var result = (DiagnosticoInfoResponse) hello.Get(new DiagnosticoInfo{Id=id} );
				var labels = new List<string> ();
				result.Capitulos.ForEach (c => labels.Add (c.Titulo));
				ViewBag.labels = new BsonArray(labels.ToArray ());
				return View (result);
			}
		}
	}

}
*/