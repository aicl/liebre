using System;
using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Diagnostico:IDocument
	{
		public Diagnostico ()
		{
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id{ get; set; }
		public string IdEmpresa { get; set; }
		public string IdPlantilla { get; set; }
		public DateTime Creado { get; set; }
		public string Responsable { get; set; }

	}
	[Route("/read/diagnostico","GET")]
	public class ReadDiagnostico:IReturn<ReadDiagnosticoResponse>
	{
		public string Id{ get; set; }
		public string IdEmpresa { get; set; }
		public string IdPlantilla { get; set; }
	}

	public class ReadDiagnosticoResponse:IHasResponseStatus
	{
		public IEnumerable<Diagnostico> Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
	}

	[Route("/create/diagnostico","POST")]
	public class CreateDiagnostico:IReturn<CreateDiagnosticoResponse>, IHasDataProperty<Diagnostico>
	{
		public Diagnostico Data { get; set; }

	}

	public class CreateDiagnosticoResponse:IHasResponseStatus
	{
		public Diagnostico Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/diagnostico","POST")]
	public class UpdateDiagnostico:IReturn<UpdateDiagnosticoResponse>, IHasDataProperty<Diagnostico>
	{
		public Diagnostico Data { get; set; }
	}

	public class UpdateDiagnosticoResponse:IHasResponseStatus
	{
		public Diagnostico Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/diagnostico","POST")]
	public class DeleteDiagnostico:IReturn<DeleteDiagnosticoResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteDiagnosticoResponse: IHasResponseStatus 
	{
		public Diagnostico Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/install/diagnostico","GET")]
	public class InstallDiagnostico:IReturn<InstallDiagnosticoResponse>
	{
		public string IdDiagnostico{ get; set; }
	}

	public class InstallDiagnosticoResponse: IHasResponseStatus
	{
		public InstallDiagnosticoResponse()
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
		public ResponseStatus ResponseStatus {get;set;}
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
