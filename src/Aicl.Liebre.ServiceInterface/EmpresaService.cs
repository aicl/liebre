using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class EmpresaService:ServiceBase
	{

		// TODO: autorizado y con privilegios de operados ( contien la llave !);
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
			return Store.CreateRegistroEmpresa (request);
		}

	}
}

