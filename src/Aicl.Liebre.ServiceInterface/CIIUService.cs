using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class CIIUService:ServiceBase
	{
		public object Get(ReadCIIU request){
			return 	Store.ReadCIIU (request);
		}
	}
}

