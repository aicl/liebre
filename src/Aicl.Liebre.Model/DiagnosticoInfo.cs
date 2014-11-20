using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	[Route("/read/diagnosticoinfo","GET")]
	public class DiagnosticoInfo
	{
		public DiagnosticoInfo ()
		{
		}
		public string Id { get; set;}
	}


	public class DiagnosticoInfoResponse: IHasResponseStatus
	{
		public DiagnosticoInfoResponse()
		{
			Guias = new List<ViewGuia> ();
			Capitulos = new List<Capitulo> ();
			Preguntas = new List<ViewPregunta> ();
			ResponseStatus = new ResponseStatus ();
		}

		public Empresa Empresa { get; set; }
		public Diagnostico Diagnostico { get; set; }
		public Plantilla Plantilla { get; set; }
		public List<Capitulo> Capitulos { get; set; }
		public List<ViewGuia> Guias { get; set; }
		public List<ViewPregunta> Preguntas { get; set; }
		public ResponseStatus ResponseStatus { get; set; }

	}
}

