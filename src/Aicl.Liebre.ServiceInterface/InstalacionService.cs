using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class InstalacionService:ServiceBase
	{

		public object Get(ReadInstalacion request){
			return ServiceBase.CreateResponse( Store.GetInstalacionResponse (request));
		}
	}
}

