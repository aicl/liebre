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

			/*if (r.Data.Estado == "green") {

				var di =Store.ReadDiagnosticoInfo (new DiagnosticoInfo{ 
					Id= r.Data.IdDiagnostico
				});

			}*/
			return r;
		}
	}
}