using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class EmpresaService:ServiceBase
	{
		public object Get(ReadEmpresa request){
			return ServiceBase.CreateResponse (Store.Get<Empresa> ());
		}


		public object Post(CreateEmpresa request)
		{
			return Store.Post<Empresa> (request.Data);
		}

		public object Post(UpdateEmpresa request)
		{
			return Store.Put<Empresa> (request.Data);
		}

		public object Post(DeleteEmpresa request)
		{
			return Store.Delete<Empresa> (request);
		}

	}
}

