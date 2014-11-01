using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Departamento:IDocument
	{
		public Departamento ()
		{
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Postal { get; set; }
		public string Nombre { get; set; }
	}

	[Route("/read/departamento","GET")]
	public class ReadDepartamento:IReturn<ReadDepartamentoResponse>
	{
		/*public string Codigo { get; set; }
		public string Nombre { get; set; }
		public Departamento Departamento { get; set; }
		public int? Skip { get; set; }
		public int? Rows { get; set; }*/
	}

	public class ReadDepartamentoResponse: IHasResponseStatus 
	{
		public IEnumerable <Departamento> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
}

