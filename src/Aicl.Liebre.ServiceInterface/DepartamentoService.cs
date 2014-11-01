using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class DepartamentoService:ServiceBase
	{
		public object Get(ReadDepartamento request){
			return Store.ReadDepartamento (request);
		}
	}
}

