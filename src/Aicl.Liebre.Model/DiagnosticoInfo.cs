using ServiceStack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

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
			Guias = new List<ViewGuiaInfo> ();
			Capitulos = new List<Capitulo> ();
			Preguntas = new List<ViewPregunta> ();
			ResponseStatus = new ResponseStatus ();
		}

		public Empresa Empresa { get; set; }
		public Diagnostico Diagnostico { get; set; }
		public Plantilla Plantilla { get; set; }
		public List<Capitulo> Capitulos { get; set; }
		public List<ViewGuiaInfo> Guias { get; set; }
		public List<ViewPregunta> Preguntas { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
	}

	public class RespuestaGuiaInfo:IDocument
	{

		public RespuestaGuiaInfo ()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdDiagnostico { get; set; }
		public string IdGuia { get; set; }
		public bool? NoAplicaChecked { get; set; }
		public string Tipo { get; set; } // tomar de la guia!!
		[BsonSerializer(typeof(DynamicSerializer))]
		public BsonValue Valor { get; set; } 

	}

	public class ViewGuiaInfo
	{
		public Guia Guia { get; set; }
		public RespuestaGuiaInfo Respuesta { get; set; }
	}

}

