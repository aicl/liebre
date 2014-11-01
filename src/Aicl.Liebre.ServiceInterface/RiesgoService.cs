using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class RiesgoService:ServiceBase
	{
		public object Get(ReadRiesgo request){
			return Store.ReadRiesgo (request);
		}
	}
}

