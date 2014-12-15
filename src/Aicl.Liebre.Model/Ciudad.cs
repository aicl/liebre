using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Ciudad:IDocument
	{
		public Ciudad ()
		{
		}

		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public Departamento Departamento { get; set; }

	}


	[Route("/read/ciudad","GET")]
	public class ReadCiudad:IReturn<ReadCiudadResponse>
	{
		/*public string Codigo { get; set; }
		public string Nombre { get; set; }
		public Departamento Departamento { get; set; }
		public int? Skip { get; set; }
		public int? Rows { get; set; }*/
	}

	public class ReadCiudadResponse: IHasResponseStatus 
	{
		public IEnumerable <Ciudad> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
}
