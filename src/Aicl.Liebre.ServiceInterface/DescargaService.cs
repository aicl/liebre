using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class DescargaService:ServiceBase
	{
		public object Get(ReadDescarga request){
		
			return	ServiceBase.CreateResponse (Store.GetByQuery<Descarga> (q=>q.IdDiagnostico==request.IdDiagnostico,
				q => q.Id, "desc"));
		}
			
		public object Post(CreateDescarga request)
		{
			return Store.PostDescarga (request);
		}

		public object Post(DeleteDescarga request)
		{
			return  Store.Delete<Descarga> (request);
		}

	}
}

