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
			return  Store.ReadDiagnosticoInfo (request);
		}

		public IHttpResult Get(DiagnosticoInfoPdf request){
			using (var client = new JsonServiceClient(AppConfig.PhantonjApiUrl)){
				try{
					var httpResponse = client.Get<HttpWebResponse>(new OpenShift.Model.DiagnosticoInfoPdf{Id= request.Id, Norma=request.Norma});
					var responseStream= httpResponse.GetResponseStream();
					var cd = new System.Net.Mime.ContentDisposition {Inline=true};
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