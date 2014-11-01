using ServiceStack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Aicl.Liebre.Model
{
	public class Riesgo:IDocument
	{
		public Riesgo ()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Clase { get; set; }
		public double Minimo { get; set; }
		public double Maximo { get; set; }
		public double Tarifa { get; set; }
		public string Tipo { get; set; }
		public string Ejemplo { get; set; }
	}

	[Route("/read/riesgo","GET")]
	public class ReadRiesgo:IReturn<ReadRiesgoResponse>
	{
		/*public string Codigo { get; set; }
		public string Nombre { get; set; }
		public Departamento Departamento { get; set; }
		public int? Skip { get; set; }
		public int? Rows { get; set; }*/
	}

	public class ReadRiesgoResponse: IHasResponseStatus 
	{
		public IEnumerable <Riesgo> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
}