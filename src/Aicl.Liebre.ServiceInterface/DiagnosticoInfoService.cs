using Aicl.Liebre.Model;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{

	public class DiagnosticoInfoService:ServiceBase
	{
		public object Get(DiagnosticoInfo request){
			var response= Store.ReadDiagnosticoInfo (request);
			return response;
		}

		public DiagnosticoInfoPdfResponse Get(DiagnosticoInfoPdf request){
			var phantom = new Phantomjs ();

			var outputfile = PathUtils.CombinePaths("~","App_Data", request.Id+".pdf").MapHostAbsolutePath ();

			var exitCode = phantom.Execute (@"{0}/DiagnosticoInfo/Index?Id={1}".Fmt (Request.GetBaseUrl ().Replace ("/lbr-api", ""), request.Id), outputfile);
			var r = new DiagnosticoInfoPdfResponse { ExitCode = exitCode };

			if (exitCode != 0) {
				r.ResponseStatus.Message = phantom.StandardError;
			}

			return r;

		}
	}
}

