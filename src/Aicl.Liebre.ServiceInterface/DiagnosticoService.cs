using System;
using Aicl.Liebre.Model;
using ServiceStack;
using System.Linq.Expressions;

namespace Aicl.Liebre.ServiceInterface
{
	public class DiagnosticoService:ServiceBase
	{
		public object Get(ReadDiagnostico request){

			Expression<Func<Diagnostico, bool>> f = (r) => 
				!request.Id.IsNullOrEmpty () ?
				r.Id == request.Id :
				(!request.IdEmpresa.IsNullOrEmpty () ?
					r.IdEmpresa == request.IdEmpresa:
					(request.IdPlantilla.IsNullOrEmpty() || r.IdPlantilla==request.IdPlantilla ));

			return ServiceBase.CreateResponse (Store.GetByQuery<Diagnostico> (f));
		}


		public object Post(CreateDiagnostico request)
		{
			return Store.Post<Diagnostico> (request.Data);
		}

		public object Post(UpdateDiagnostico request)
		{
			return Store.Put<Diagnostico> (request.Data);
		}

		public object Post(DeleteDiagnostico request)
		{

			var r = Store.Delete<Respuesta> (q=> q.IdDiagnostico== request.Id);
			if (!r.WriteResult.Ok)
				return r;

			var g = Store.Delete<RespuestaGuia> (q => q.IdDiagnostico == request.Id);

			return !g.WriteResult.Ok ? (object)g : Store.Delete<Diagnostico> (request);
							
		}

		public object Get(InstallDiagnostico request){
			return Store.GetInstallDiagnosticoResponse (request);
		}

	}
}

