using ServiceStack;

namespace Aicl.Liebre.Model
{
	public class RespuestaCuestionario
	{
		public RespuestaCuestionario ()
		{
		}
		public Descarga Descarga{ get; set;}
		public Respuesta Respuesta {get;set;}


	}

	[Route("/save/respuesta","POST,PUT")]
	public class SaveRespuesta:IReturn<SaveRespuestaResponse>
	{
		public RespuestaCuestionario Data { get; set; }
	}

	public class SaveRespuestaResponse: IHasResponseStatus 
	{
		public Respuesta Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}

