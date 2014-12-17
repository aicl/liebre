using Aicl.Liebre.Model;
using Aicl.Liebre.Data;
using ServiceStack.FluentValidation;

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
			var v = new DiagnosticoValidator ( Store);
			v.ValidateAndThrow (new Diagnostico{ Id = request.Id }, "delete");
			return Store.Delete<Diagnostico> (request);					
		}
	}
}
	