using System;
using Aicl.Liebre.Model;
using ServiceStack;
using System.IO;
using ServiceStack.Web;
using System.Net;

namespace Aicl.Liebre.ServiceInterface
{
	public class DiagnosticoInfoService:ServiceBase
	{
		public object Get(DiagnosticoInfo request){
			var response= Store.ReadDiagnosticoInfo (request);
			return response;
		}

		public IHttpResult Get(DiagnosticoInfoPdf request){
			using (var client = new JsonServiceClient(AppConfig.PhantonjsURL)){
				try{
					var httpResponse = client.Get<HttpWebResponse>("/DiagnosticoInfoPdf/Index?id={0}".Fmt(request.Id));
					var responseStream= httpResponse.GetResponseStream();
					Response.AddHeader("Content-Disposition",httpResponse.Headers["Content-Disposition"]);
					return new HttpResult(responseStream, httpResponse.ContentType);
				}
				catch(Exception e){
					return new HttpError (e.Message);
				}
			}
		}
	}
}