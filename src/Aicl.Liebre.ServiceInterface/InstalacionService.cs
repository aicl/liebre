using Aicl.Liebre.Model;
using System;

namespace Aicl.Liebre.ServiceInterface
{
	public class InstalacionService:ServiceBase
	{

		public object Get(ReadInstalacion request){
			return ServiceBase.CreateResponse( Store.GetInstalacionResponse (request));
		}
			
		public object Post(UpdateCuestionario request){
			Console.WriteLine ("{0} {1}",this.Request, request.Data.RespuestasGuias[0]);
			return Store.UpdateCuestionario (request);
		}
	}
}
