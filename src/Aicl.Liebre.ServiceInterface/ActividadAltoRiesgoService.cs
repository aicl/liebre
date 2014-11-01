using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class ActividadAltoRiesgoService:ServiceBase
	{
		public object Get(ReadActividadAltoRiesgo request){
			return Store.ReadActividaAltoRiesgo (request);
		}
	}
}

