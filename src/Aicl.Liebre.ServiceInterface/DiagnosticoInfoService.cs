using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{

	public class DiagnosticoInfoService:ServiceBase
	{
		public DiagnosticoInfoResponse Get(DiagnosticoInfo request){
			return Store.ReadDiagnosticoInfo (request);
		}
	}
}

