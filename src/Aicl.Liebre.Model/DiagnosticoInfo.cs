using ServiceStack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack.Web;

namespace Aicl.Liebre.Model
{
	[Route("/read/diagnosticoinfo","GET")]
	public class DiagnosticoInfo:IReturn<DiagnosticoInfoResponse>
	{
		public DiagnosticoInfo ()
		{
			Norma = "Decreto";
		}
		public string Id { get; set;}
		public string Norma { get; set; }
	}

	[Route("/read/diagnosticoinfopdf","GET")]
	public class DiagnosticoInfoPdf:IReturn<IHttpResult>
	{
		public DiagnosticoInfoPdf ()
		{
			Norma = "Decreto";
		}
		public string Id { get; set;}
		public string Norma { get; set; }
	}

	[Route("/create/diagnosticoinfo","POST")]
	public class CreateDiagnosticoInfo:IReturn<CreateDiagnosticoInfoResponse>
	{
		public string Id{ get; set; }
	}
		
	public class CreateDiagnosticoInfoResponse:IHasResponseStatus
	{
		public CreateDiagnosticoInfoResponse(){
			ResponseStatus = new ResponseStatus ();
		}

		public ResponseStatus ResponseStatus {
			get;
			set;
		}

	}

	public class DiagnosticoInfoResponse: IHasResponseStatus
	{
		public DiagnosticoInfoResponse()
		{
			//Guias = new List<ViewGuiaInfo> ();
			Capitulos = new List<CapituloInfo> ();
			Preguntas = new List<ViewPreguntaInfo> ();
			ResponseStatus = new ResponseStatus ();
			Normas = new List<Norma> ();
		}

		public EmpresaLogo Empresa { get; set; }
		public Diagnostico Diagnostico { get; set; }
		public Plantilla Plantilla { get; set; }
		public List<CapituloInfo> Capitulos { get; set; }
		//public List<ViewGuiaInfo> Guias { get; set; }
		public List<ViewPreguntaInfo> Preguntas { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public string Norma { get; set; }
		public List<Norma> Normas { get; set; }
	}

	public class CapituloInfo:Capitulo
	{
		public CapituloInfo(){

		}

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


}