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
			return r;

			/*if (r.Data.Estado == "green") {

				var di =Store.ReadDiagnosticoInfo (new DiagnosticoInfo{ 
					Id= r.Data.IdDiagnostico
				});

			}*/
			/*
			var files =Informant.GetAllFileInfo<DiagnosticoInfo> ();
			foreach (var f in files)
				Informant.GetHtml<DiagnosticoInfo> (new DiagnosticoInfo (), f);

			return new {};*/
		}
	}
}