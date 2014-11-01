using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class CiudadService:ServiceBase
	{

		public object Get(ReadCiudad request){
			return Store.ReadCiudad (request);
		}
	}
}

