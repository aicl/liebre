using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class EmpresaService:ServiceBase
	{
		public object Get(ReadEmpresa request){
			return ServiceBase.CreateResponse (Store.Get<Empresa> (q=>q.Nombre));
		}

		// TODO: autorizado y con privilegios de operador!
		public object Post(CreateEmpresa request)
		{
			return Store.CreateEmpresa (request);
		}


		// TODO: autorizado y con privilegios de operador!
		public object Post(UpdateEmpresa request)
		{
			return Store.UpdateEmpresa (request);
		}

		// TODO: autorizado y con privilegios de operador!
		public object Post(DeleteEmpresa request)
		{
			return Store.DeleteEmpresa (request);
		}



		public object Post(CreateRegistroEmpresa request)
		{
			request.Data.Llave = Aicl.Liebre.Data.Store.CreateRandomPassword (48);
			request.Data.IdPlan = null;
			return Store.Post<Empresa> (request.Data);
		}

	}
}

