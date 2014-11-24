using Aicl.Liebre.Model;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{

	public class DiagnosticoInfoService:ServiceBase
	{
		public DiagnosticoInfoResponse Get(DiagnosticoInfo request){
			System.Console.WriteLine ("SS DiagnosticoInfo");
			var response= Store.ReadDiagnosticoInfo (request);
			return response;
		}

		public DiagnosticoInfoResponse Get(DiagnosticoInfoPdf request){
			var phantom = new Phantomjs ();
			System.Console.WriteLine ("SS DiagnosticoInfoPDF");

			var outputfile = PathUtils.CombinePaths("~","App_Data", request.Id+".pdf").MapHostAbsolutePath ();
			System.Console.WriteLine (outputfile);
			System.Console.WriteLine (@"{0}/DiagnosticoInfo/Index?Id={1}".Fmt(Request.GetBaseUrl().Replace("/lbr-api",""),request.Id));
			System.Console.WriteLine(phantom.Execute (@"{0}/DiagnosticoInfo/Index?Id={1}".Fmt(Request.GetBaseUrl().Replace("/lbr-api",""),request.Id), outputfile));
			System.Console.WriteLine (phantom.StandardError);
			System.Console.WriteLine (phantom.StandardOutput);
			return new DiagnosticoInfoResponse ();
		}
	}
}

