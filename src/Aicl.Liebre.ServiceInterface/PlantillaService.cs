using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class PlantillaService:ServiceBase
	{

		public object Get(ReadPlantilla request){
			return ServiceBase.CreateResponse (Store.Get<Plantilla> ());
		}

		public object Get(SinglePlantilla request){

			return ServiceBase.CreateResponse (Store.GetById<Plantilla> (request));
		}

		public object Post(CreatePlantilla request)
		{
			return Store.Post(request.Data);
		}

		public object Post(UpdatePlantilla request)
		{
			return Store.Put (request.Data);
		}

		public object Post(DeletePlantilla request)
		{
			return Store.Delete<Plantilla> (request);

		}
			
	}
}

