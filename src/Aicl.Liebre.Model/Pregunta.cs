using ServiceStack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Pregunta:IDocument
	{
		public Pregunta ()
		{
			IdGuias = new List<string> ();
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get ; set;}
		public string IdCapitulo { get; set;}
		public string Numeral { get; set; }
		public string Enunciado { get; set; }
		public string Metodo { get; set; } // metodo verificacion
		public bool NoAplicaDisponible { get; set; }
		public List<string> IdGuias { get; set; }
	}

	[Route("/read/pregunta","GET")]
	public class ReadPregunta:IReturn<ReadPreguntaResponse>
	{
		public string IdCapitulo { get; set; }
	}

	public class ReadPreguntaResponse: IHasResponseStatus 
	{
		public IEnumerable <Pregunta> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	[Route("/create/pregunta","POST")]
	public class CreatePregunta:IReturn<CreatePreguntaResponse>,IHasDataProperty<Pregunta>
	{
		public Pregunta Data { get; set; }
	}

	public class CreatePreguntaResponse: IHasResponseStatus 
	{
		public Pregunta Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/pregunta","POST")]
	public class UpdatePregunta:IReturn<UpdatePreguntaResponse>,IHasDataProperty<Pregunta>
	{
		public Pregunta Data { get; set; }
	}

	public class UpdatePreguntaResponse: IHasResponseStatus 
	{
		public Pregunta Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/pregunta","POST")]
	public class DeletePregunta:IReturn<DeletePreguntaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeletePreguntaResponse: IHasResponseStatus 
	{
		public Pregunta Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}


}

