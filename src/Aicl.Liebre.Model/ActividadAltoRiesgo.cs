using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class ActividadAltoRiesgo:IDocument
	{
		public ActividadAltoRiesgo ()
		{
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Descripcion { get; set; }
	}

	[Route("/read/actividadaltoriesgo","GET")]
	public class ReadActividadAltoRiesgo:IReturn<ReadActividadAltoRiesgoResponse>
	{
		/*public string Codigo { get; set; }
		public string Nombre { get; set; }
		public Departamento Departamento { get; set; }
		public int? Skip { get; set; }
		public int? Rows { get; set; }*/
	}

	public class ReadActividadAltoRiesgoResponse: IHasResponseStatus 
	{
		public IEnumerable <ActividadAltoRiesgo> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

}

