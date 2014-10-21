using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class InstalacionService:ServiceBase
	{

		public object Get(ReadInstalacion request){
			return ServiceBase.CreateResponse( Store.GetInstalacionResponse (request));
		}


		public object Post(SaveRespuesta request){
			System.Console.WriteLine (request);
			return null;
		}

		public object Post(SaveRespuestaGuia request){
			System.Console.WriteLine (request);
			return null;
		}

	}
}

