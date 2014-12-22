using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class CuestionarioService:ServiceBase
	{

		public object Get(ReadCuestionario request){
			return ServiceBase.CreateResponse( Store.DownloadCuestionario (request));
		}
			
		public object Post(UpdateCuestionario request){
			var r= Store.UpdateCuestionario (request);
			if (r.Data.Estado == "green") {
				var di =Store.ReadDiagnosticoInfo (new DiagnosticoInfo{ 
					Id= r.Data.IdDiagnostico
				});
				//var httpResponse =
				//client.Get<HttpWebResponse>("/phn-api/read/diagnosticoinfopdf?Id={0}&Norma={1}".Fmt(request.Id,request.Norma));
				//Remote.Publish<DiagnosticoInfo> (di);
			}
			return r;
		}
	}
}