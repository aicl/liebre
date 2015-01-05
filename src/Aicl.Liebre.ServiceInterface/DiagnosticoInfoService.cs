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

		public object Post(CreateDiagnosticoInfo request)
		{
			var di =Store.ReadDiagnosticoInfo (new DiagnosticoInfo{ 
				Id= request.Id,
			});

			var rq = new OpenShift.Model.DiagnosticoInfo {
				ApiKey = AppConfig.PhantonjsApikey,
				Id = di.Diagnostico.Id,
				IdEmpresa = di.Empresa.Id,
				Revision= di.Diagnostico.Revision
			};

			rq.Mail.Html = HtmlBodyMail.RenderToHtml (di, typeof (DiagnosticoInfo));
			Console.WriteLine(rq.Mail.Html);

			var files =Informant.GetAllFileInfo<DiagnosticoInfo> ();
			foreach (var f in files)
				rq.Informes.Add (new OpenShift.Model.Informe {
					Formato =	Informant.GetUtf8Bytes (di, f),
					Nombre = f.Name
				});

			rq.Mail.To.Add (di.Empresa.Email);
			rq.Mail.Subject = "Informe Diagnostico SG-SST";

			using (var client = new JsonServiceClient(AppConfig.PhantonjsOneWayUrl)){
				try{

					client.Post(rq);

				}
				catch(Exception e){
					throw new HttpError (e.Message);
				}
			}


			return new CreateDiagnosticoInfoResponse ();
		}

	}
}