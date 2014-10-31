using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class CIIU:IDocument
	{
		public CIIU ()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Seccion { get; set; }
		public string Division { get; set; }
		public string Grupo { get; set; }
		public string Clase { get; set; }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
	}

	[Route("/read/ciiu","GET")]
	public class ReadCIIU:IReturn<ReadCIIUResponse>
	{
		public string Codigo { get; set; }
		public string Seccion { get; set; }
		public string Descripcion { get; set; }
		public int? Skip { get; set; }
		public int? Rows { get; set; }
	}

	public class ReadCIIUResponse: IHasResponseStatus 
	{
		public IEnumerable <CIIU> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

}

