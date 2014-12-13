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
					var httpResponse = client.Get<HttpWebResponse>("/phn-api/read/diagnosticoinfopdf?Id={0}&Norma={1}".Fmt(request.Id,request.Norma));
					var responseStream= httpResponse.GetResponseStream();
					var cd = new System.Net.Mime.ContentDisposition {
						Inline=true
					};
					Response.AddHeader ("Content-Disposition", cd.ToString ());
					return new HttpResult(responseStream, httpResponse.ContentType);
				}
				catch(Exception e){
					return new HttpError (e.Message);
				}
			}
		}
	}
}