using ServiceStack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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


	[Route("/read/cuestionario","GET")]
	public class ReadCuestionario:IReturn<ReadCuestionarioResponse>
	{
		public string Token{ get; set; }
	}

	public class ReadCuestionarioResponse: IHasResponseStatus
	{
		public ReadCuestionarioResponse()
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


	public class PreguntaDescarga{

		public PreguntaDescarga ()
		{
			IdGuias = new List<string> ();
			Preguntas = new List<string> ();
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get ; set;}
		public string IdCapitulo { get; set;}
		public string Numeral { get; set; }
		public string Enunciado { get; set; }
		public string Metodo { get; set; } // metodo verificacion
		public bool NoAplicaDisponible { get; set; }
		public List<string> IdGuias { get; set; }
		public List<string> Preguntas { get; set; }

	}

	public class ViewPregunta
	{
		public PreguntaDescarga Pregunta { get; set; }
		public Respuesta Respuesta { get; set; }
	}

	public class ViewGuia
	{
		public Guia Guia { get; set; }
		public RespuestaGuia Respuesta { get; set; }
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