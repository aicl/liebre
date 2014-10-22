using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Cuestionario
	{
		public Cuestionario ()
		{
			Descarga = new Descarga ();
			Respuestas = new List<Respuesta> ();
			RespuestasGuias = new List<RespuestaGuia> ();
		}
		public Descarga Descarga{ get; set;}
		public List<Respuesta> Respuestas {get;set;}
		public List<RespuestaGuia> RespuestasGuias {get;set;}
	}

	[Route("/update/cuestionario","POST,PUT")]
	public class UpdateCuestionario:IReturn<UpdateCuestionarioResponse>
	{
		public Cuestionario Data { get; set; }
	}

	public class UpdateCuestionarioResponse: IHasResponseStatus 
	{
		public Respuesta Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}
}

