using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class GuiaService:ServiceBase
	{
		public object Get(ReadGuia request){

			return ServiceBase.CreateResponse (
				Store.GetByQuery<Guia> (q => q.IdPlantilla == request.IdPlantilla,q=>q.Id));
		}

		public object Post(CreateGuia request)
		{
			return Store.Post<Guia> (request.Data);
		}

		public object Post(UpdateGuia request)
		{
			return Store.Put<Guia> (request.Data);
		}

		public object Post(DeleteGuia request)
		{
			return Store.Delete<Guia> (request);

		}

	}
}

