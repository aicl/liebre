using ServiceStack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class RespuestaGuia:IDocument
	{

		public RespuestaGuia ()
		{
			//Valor = new Dictionary<string, object> ();
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdDiagnostico { get; set; }
		public string IdGuia { get; set; }
		public bool? NoAplicaChecked { get; set; }
		//public Dictionary<string,object> Valor { get; set; } // este es el que debe quedar!
		public string Valor { get; set; }
		public string Tipo { get; set; } // tomar de la guia!!!

	}

	[Route("/read/respuestaguia","GET")]
	public class ReadRespuestaGuia:IReturn<ReadRespuestaGuiaResponse>
	{
		public string Id { get; set; }
		[Required]
		public string IdDiagnostico { get; set; }
		public List<string> IdGuias { get; set; }
	}

	public class ReadRespuestaGuiaResponse:IHasResponseStatus
	{
		public ReadRespuestaGuiaResponse()
		{
			RespuestasGuias = new List<RespuestaGuia> ();
		}

		public IEnumerable <RespuestaGuia> RespuestasGuias { get; set;}
		public ResponseStatus ResponseStatus { get; set;}

	}

	[Route("/create/respuestaguia","POST")]
	public class CreateRespuestaGuia:IReturn<CreateRespuestaGuiaResponse>, IHasDataProperty<RespuestaGuia>
	{
		public RespuestaGuia Data { get; set; }
	}

	public class CreateRespuestaGuiaResponse:IHasResponseStatus
	{
		public RespuestaGuia Data { get; set; }
		public ResponseStatus ResponseStatus { get; set;}
		public WriteResult WriteResult{ get; set; }
	}


	[Route("/update/respuestaguia","POST")]
	public class UpdateRespuestaGuia:IReturn<UpdateRespuestaGuiaResponse>, IHasDataProperty<RespuestaGuia>
	{
		public RespuestaGuia Data { get; set; }
	}

	public class UpdateRespuestaGuiaResponse:IHasResponseStatus
	{
		public RespuestaGuia Data { get; set; }
		public ResponseStatus ResponseStatus { get; set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/respuestaguia","POST")]
	public class DeleteRespuestaGuia:IReturn<DeleteRespuestaGuiaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteRespuestaGuiaResponse: IHasResponseStatus 
	{
		public RespuestaGuia Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}

