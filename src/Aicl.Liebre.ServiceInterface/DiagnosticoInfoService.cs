using System;
using Aicl.Liebre.Model;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{
	//[ClientCanSwapTemplates]
	//[DefaultView("DiagnosticoInfo")]
	public class DiagnosticoInfoService:ServiceBase
	{
		public object Get(DiagnosticoInfo request){
			return Store.ReadDiagnosticoInfo (request);
		}
	}
}

