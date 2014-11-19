using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Requisito
	{
		public Requisito ()
		{
		}

		public string Codigo { get; set; }
		public string Descripcion { get; set; }
	}


	[Route("/read/requisito","GET")]
	public class ReadRequisito:IReturn<ReadRequisitoResponse>
	{
	}

	public class ReadRequisitoResponse: IHasResponseStatus 
	{
		public IEnumerable <Requisito> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
}
