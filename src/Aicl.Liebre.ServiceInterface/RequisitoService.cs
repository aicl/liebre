using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class RequisitoService:ServiceBase
	{
		public object Get(ReadRequisito request)
		{
			return Store.ReadRequisito (request);
		}
	}
}

