using System;
using Aicl.Liebre.Model;
using MongoDB.Driver.Builders;

namespace Aicl.Liebre.ServiceInterface
{
	public class PlantillaService:ServiceBase
	{

		public object Get(ReadPlantilla request){
			return ServiceBase.CreateResponse (Store.Get<Plantilla> (q=>q.Id));
		}

		public object Get(SinglePlantilla request){

			return ServiceBase.CreateResponse (Store.Single<Plantilla> (Query<Plantilla>.EQ (q => q.Id, request.Id)));
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

		public object Post(ClonePlantilla request)
		{
			return Store.ClonarPlantilla(request.Data);
		}

	}
}

