using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class DiagnosticoService:ServiceBase
	{
		public object Get(ReadDiagnostico request)
		{
			return ServiceBase.CreateResponse (Store.ReadDiagnostico (request));
		}


		public object Post(CreateDiagnostico request)
		{
			return Store.PostDiagnostico(request);
		}

		public object Post(UpdateDiagnostico request)
		{
			return Store.PutDiagnostico(request);
		}

		public object Post(DeleteDiagnostico request)
		{

			var r = Store.Delete<Respuesta> (q=> q.IdDiagnostico== request.Id);
			if (!r.WriteResult.Ok)	return r;

			var g = Store.Delete<RespuestaGuia> (q => q.IdDiagnostico == request.Id);
			return !g.WriteResult.Ok ? (object)g : Store.Delete<Diagnostico> (request);					
		}
	}
}
	