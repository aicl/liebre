using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceStack;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Guia:IDocument
	{
		public Guia ()
		{
			Tipo = "string";
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdPlantilla { get; set; }
		public string Enunciado { get; set; }
		public string Tipo { get; set; } 
	}

	[Route("/read/guia","GET")]
	public class ReadGuia:IReturn<ReadGuiaResponse>
	{
		public string IdPlantilla { get; set; }
	}

	public class ReadGuiaResponse: IHasResponseStatus 
	{
		public IEnumerable <Guia> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	[Route("/create/guia","POST")]
	public class CreateGuia:IReturn<CreateGuiaResponse>,IHasDataProperty<Guia>
	{
		public Guia Data { get; set; }
	}

	public class CreateGuiaResponse: IHasResponseStatus 
	{
		public Guia Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/guia","POST")]
	public class UpdateGuia:IReturn<UpdateGuiaResponse>,IHasDataProperty<Guia>
	{
		public Guia Data { get; set; }
	}

	public class UpdateGuiaResponse: IHasResponseStatus 
	{
		public Guia Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/guia","POST")]
	public class DeleteGuia:IReturn<DeleteGuiaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteGuiaResponse: IHasResponseStatus 
	{
		public Guia Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}


