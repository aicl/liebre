using System.Linq;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class PreguntaService:ServiceBase
	{
		public object Get(ReadPregunta request){

			return ServiceBase.CreateResponse (
				Store.ReadPregunta(request));
		}

		public object Post(CreatePregunta request)
		{
			return Store.Post(request.Data);
		}

		public object Post(UpdatePregunta request)
		{
			return Store.Put(request.Data);
		}

		public object Post(DeletePregunta request)
		{
			return Store.Delete<Pregunta> (request);

		}

	}
}

