using Aicl.Liebre.Model;
using ServiceStack;
using System.IO;
using ServiceStack.Web;

namespace Aicl.Liebre.ServiceInterface
{

	public class DiagnosticoInfoService:ServiceBase
	{
		public object Get(DiagnosticoInfo request){
			var response= Store.ReadDiagnosticoInfo (request);
			return response;
		}

		public IHttpResult Get(DiagnosticoInfoPdf request){
			var phantom = new Phantomjs ();
			var outputfile = PathUtils.CombinePaths("~","App_Data", request.Id+".pdf").MapHostAbsolutePath ();
			var exitCode = phantom.Execute (@"{0}/DiagnosticoInfo/Index?Id={1}".Fmt (Request.GetBaseUrl ().Replace ("/lbr-api", ""), request.Id), outputfile);
			return exitCode != 0 ?
				(IHttpResult) new HttpError (phantom.StandardError) :
				(IHttpResult) new HttpResult (new FileInfo (outputfile), contentType: "application/pdf", asAttachment: true);
		}
	}
}

