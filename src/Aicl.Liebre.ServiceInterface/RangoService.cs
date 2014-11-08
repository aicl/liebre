using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class RangoService:ServiceBase
	{
		public object Get(ReadRango request){
			return Store.ReadRango (request);
		}
	}
}