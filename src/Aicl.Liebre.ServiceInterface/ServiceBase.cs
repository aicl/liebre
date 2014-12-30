using Aicl.Liebre.Data;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{

	public class ServiceBase:Service
	{
		public Store Store{ get; set; }

		public AppConfig AppConfig { get; set; }

		public IInformant Informant { get; set; }

		protected static object CreateResponse(object data){
			return new {
				Data=data,
				ResponseStatus= new ResponseStatus()
			};
		}
	}


}

