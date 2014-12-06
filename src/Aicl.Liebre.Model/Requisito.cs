using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Requisito
	{
		public Requisito ()
		{
			Decreto = new Referencia ();
			OHSAS = new Referencia ();
			RUC = new Referencia ();
		}

		public string Codigo { get; set; }
		public string Descripcion { get; set; }
		public Referencia Decreto { get; set; }
		public Referencia OHSAS { get; set; }
		public Referencia RUC { get; set; }
	}

	public class Referencia{
		public string Numerales {get;set;}
		public string Titulo {get;set;}
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
