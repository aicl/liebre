using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Externo:IDocument
	{
		public Externo ()
		{
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
	}


	[Route("/read/externo","GET")]
	public class ReadExterno:IReturn<ReadExternoResponse>
	{
	}

	public class ReadExternoResponse: IHasResponseStatus 
	{
		public IEnumerable <Externo> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
}