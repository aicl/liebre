using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Instalacion
	{
		public Instalacion ()
		{
		}

		public string Token { get; set; }
	}

	[Route("/read/instalacion","GET")]
	public class ReadInstalacion:IReturn<InstalacionResponse>
	{
		public string Token{ get; set; }
	}

	public class InstalacionResponse: IHasResponseStatus
	{
		public InstalacionResponse()
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
		public Descarga Descarga { get; set; }
	}
		
	public class ViewPregunta
	{
		public Pregunta Pregunta { get; set; }
		public Respuesta Respuesta { get; set; }
	}

	public class ViewGuia
	{
		public Guia Guia { get; set; }
		public RespuestaGuia Respuesta { get; set; }
	}

}
