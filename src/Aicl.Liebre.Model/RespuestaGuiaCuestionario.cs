using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class RespuestaGuiaCuestionario
	{
		public RespuestaGuiaCuestionario ()
		{
		}
		public Descarga Descarga{ get; set;}
		public List<RespuestaGuia> Respuestas {get;set;}


	}

	[Route("/save/respuestaguia","POST,PUT")]
	public class SaveRespuestaGuia:IReturn<SaveRespuestaGuiaResponse>	{
		public RespuestaGuiaCuestionario Data { get; set; }
	}

	public class SaveRespuestaGuiaResponse: IHasResponseStatus 
	{
		public RespuestaGuia Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}

