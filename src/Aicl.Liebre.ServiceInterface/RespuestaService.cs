using System;
using Aicl.Liebre.Model;
using System.Linq.Expressions;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{
	public class RespuestaService:ServiceBase
	{
		public object Get(ReadRespuesta request){

			Expression<Func<Respuesta, bool>> f = (r) => 
				!request.Id.IsNullOrEmpty () ?
				r.Id == request.Id :
				(!request.IdDiagnostico.IsNullOrEmpty () ?
					r.IdDiagnostico == request.IdDiagnostico :
					(request.IdPreguntas.Count <= 0 || request.IdPreguntas.Contains (r.IdPregunta)));

			return ServiceBase.CreateResponse (Store.GetByQuery<Respuesta> (f));
		}


		public object Post(CreateRespuesta request)
		{
			return Store.Post (request.Data);
		}

		public object Post(UpdateRespuesta request)
		{
			return Store.Put (request.Data);
		}

		public object Post(DeleteRespuesta request)
		{
			return Store.Delete<Respuesta> (request);

		}

	}
}

