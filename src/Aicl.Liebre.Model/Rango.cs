using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Rango:IDocument
	{
		public Rango ()
		{
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public int MaxValue { get; set; }
		public int MinValue { get; set; }

	}


	[Route("/read/rango","GET")]
	public class ReadRango:IReturn<ReadRangoResponse>
	{
		/*public string Codigo { get; set; }
		public string Nombre { get; set; }
		public Departamento Departamento { get; set; }
		public int? Skip { get; set; }
		public int? Rows { get; set; }*/
	}

	public class ReadRangoResponse: IHasResponseStatus 
	{
		public IEnumerable <Rango> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
}
