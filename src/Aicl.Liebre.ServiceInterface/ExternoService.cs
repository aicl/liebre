using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class ExternoService:ServiceBase
	{
		public object Get(ReadExterno request){
			return Store.ReadExterno (request);
		}
	}
}

