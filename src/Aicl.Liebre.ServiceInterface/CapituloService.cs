using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class CapituloService:ServiceBase
	{

		public object Get(ReadCapitulo request){

			return ServiceBase.CreateResponse (
				Store.GetByQuery<Capitulo> (q => q.IdPlantilla == request.IdPlantilla));
		}


		public object Post(CreateCapitulo request)
		{
			return Store.Post(request.Data);
		}

		public object Post(UpdateCapitulo request)
		{
			return Store.Put (request.Data);
		}

		public object Post(DeleteCapitulo request)
		{
			return Store.Delete<Capitulo> (request);

		}


	}
}

