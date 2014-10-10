using System;
using Aicl.Liebre.Model;
using System.Linq.Expressions;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{
	public class RespuestaGuiaGuiaService:ServiceBase
	{
		public object Get(ReadRespuestaGuia request){

			Expression<Func<RespuestaGuia, bool>> f = (r) => 
				!request.Id.IsNullOrEmpty () ?
				r.Id == request.Id :
				r.IdDiagnostico == request.IdDiagnostico &&
				(request.IdGuias.Count <= 0 || request.IdGuias.Contains (r.IdGuia));

			return ServiceBase.CreateResponse (Store.GetByQuery<RespuestaGuia> (f));
		}


		public object Post(CreateRespuestaGuia request)
		{
			return Store.Post (request.Data);
		}

		public object Post(UpdateRespuestaGuia request)
		{
			return Store.Put (request.Data);
		}

		public object Post(DeleteRespuestaGuia request)
		{
			return Store.Delete<RespuestaGuia> (request);

		}
	}
}

