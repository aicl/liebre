using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Respuesta:IDocument
	{
		public Respuesta ()
		{
			Respuestas = new List<bool> ();
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdDiagnostico { get; set; }
		public string IdPregunta { get; set; }
		public bool? NoAplicaChecked { get; set; }
		public short? Valor { get; set; }
		public List<bool> Respuestas{ get; set; }
	}

	[Route("/read/respuesta","GET")]
	public class ReadRespuesta:IReturn<ReadRespuestaResponse>
	{
		public ReadRespuesta()
		{
			IdPreguntas = new List<string> ();
		}
		public string Id { get; set; }
		public string IdDiagnostico { get; set; }
		public List<string> IdPreguntas { get; set; }
	}

	public class ReadRespuestaResponse:IHasResponseStatus
	{
		public IEnumerable <Respuesta> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}


	[Route("/create/respuesta", "POST")]
	public class CreateRespuesta:IReturn<CreateRespuestaResponse>, IHasDataProperty<Respuesta>
	{
		public Respuesta Data { get; set; }
	}

	public class CreateRespuestaResponse:IHasResponseStatus
	{
		public Respuesta Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/respuesta", "POST")]
	public class UpdateRespuesta:IReturn<UpdateRespuestaResponse>, IHasDataProperty<Respuesta>
	{
		public Respuesta Data { get; set; }
	}

	public class UpdateRespuestaResponse:IHasResponseStatus
	{
		public Respuesta Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/respuesta","POST")]
	public class DeleteRespuesta:IReturn<DeleteRespuestaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteRespuestaResponse: IHasResponseStatus 
	{
		public Respuesta Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}
		
}


