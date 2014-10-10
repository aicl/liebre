using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Capitulo:IDocument
	{
		public Capitulo ()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdPlantilla { get; set; }
		public string Titulo { get; set; }
		public string Estandar { get; set; }

	}

	[Route("/read/capitulo","GET")]
	public class ReadCapitulo:IReturn<ReadCapituloResponse>
	{
		public string IdPlantilla { get; set; }
	}

	public class ReadCapituloResponse: IHasResponseStatus 
	{
		public IEnumerable <Capitulo> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	[Route("/create/capitulo","POST")]
	public class CreateCapitulo:IReturn<CreateCapituloResponse>,IHasDataProperty<Capitulo>
	{
		public Capitulo Data { get; set; }
	}

	public class CreateCapituloResponse: IHasResponseStatus 
	{
		public Capitulo Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}


	[Route("/update/capitulo","POST")]
	public class UpdateCapitulo:IReturn<UpdateCapituloResponse>,IHasDataProperty<Capitulo>
	{
		public Capitulo Data { get; set; }
	}

	public class UpdateCapituloResponse: IHasResponseStatus 
	{
		public Capitulo Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/capitulo","POST")]
	public class DeleteCapitulo:IReturn<DeleteCapituloResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteCapituloResponse: IHasResponseStatus 
	{
		public Capitulo Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}

