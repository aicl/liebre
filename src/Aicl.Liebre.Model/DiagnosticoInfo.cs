using ServiceStack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Linq;

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

	[Route("/read/diagnosticoinfopdf","GET")]
	public class DiagnosticoInfoPdf
	{
		public DiagnosticoInfoPdf ()
		{
		}
		public string Id { get; set;}
	}

	public class DiagnosticoInfoPdfResponse: IHasResponseStatus
	{
		public DiagnosticoInfoPdfResponse(){
			ResponseStatus = new ResponseStatus ();
		}

		public int ExitCode { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
	}

	public class DiagnosticoInfoResponse: IHasResponseStatus
	{
		public DiagnosticoInfoResponse()
		{
			Guias = new List<ViewGuiaInfo> ();
			Capitulos = new List<CapituloInfo> ();
			Preguntas = new List<ViewPreguntaInfo> ();
			ResponseStatus = new ResponseStatus ();
			Requisitos = new List<ViewRequisito> ();
		}

		public Empresa Empresa { get; set; }
		public Diagnostico Diagnostico { get; set; }
		public Plantilla Plantilla { get; set; }
		public List<CapituloInfo> Capitulos { get; set; }
		public List<ViewGuiaInfo> Guias { get; set; }
		public List<ViewPreguntaInfo> Preguntas { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public List<ViewRequisito> Requisitos { get; set; }
	}

	public class CapituloInfo:Capitulo
	{
		public CapituloInfo(){
			//Preguntas = new List<ViewPreguntaInfo> ();
		}
		//public List<ViewPreguntaInfo> Preguntas { get; set; }

		public int TotalQ { get; set; }
		public int TotalR { get; set; }

	}

	public class RespuestaGuiaInfo:IDocument
	{

		public RespuestaGuiaInfo ()
		{
			//Valor = BsonNull.Value;
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdDiagnostico { get; set; }
		public string IdGuia { get; set; }
		public bool? NoAplicaChecked { get; set; }
		public string Tipo { get; set; } // tomar de la guia!!
		[BsonSerializer(typeof(CustomSerializer))]
		public string Valor { get; set; } 

	}

	public class ViewGuiaInfo
	{
		public Guia Guia { get; set; }
		public RespuestaGuiaInfo Respuesta { get; set; }
	}

	public class ViewPreguntaInfo{
		public ViewPreguntaInfo(){
			Guias = new List<ViewGuiaInfo> ();
		}
		public Pregunta Pregunta { get; set; }
		public Respuesta Respuesta { get; set; }
		public List<ViewGuiaInfo> Guias { get; set; }
	}

	public class ViewRequisito:Requisito{

		public int TotalQ { get; set; }
		public int TotalR { get; set; }
	}
}

/*
 * public int TotalQ { 
			get { 	return Preguntas.Count; 	} 
		}
		public int TotalR{
			get {
				return Preguntas.Count (q => q.Respuesta.Valor == 1);
			}
		}
 */