using ServiceStack;
using System.Collections.Generic;

namespace Aicl.Liebre.Model
{
	public class Norma:IDocument
	{
		public Norma ()
		{
			Items = new List<NormaItem> ();
			Grupos = new List<NormaGrupo> ();
		}
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Anotacion { get; set; }
		public string Titulo { get; set; }
		public string Contenido { get; set; }
		public string Alias { get; set; }
		public int TotalQ { get; set; }
		public int TotalR { get; set; }
		public List<NormaItem> Items { get; set; }
		public List<NormaGrupo> Grupos { get; set; }

	}

	[Route("/read/norma","GET")]
	public class ReadNorma:IReturn<ReadNormaResponse>
	{
	}

	public class ReadNormaResponse: IHasResponseStatus 
	{
		public IEnumerable <Norma> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}
		
	public class NormaItem{
		public string Codigo{ get; set; }
		public string Titulo{ get; set; }
		public int TotalQ { get; set; }
		public int TotalR { get; set; }
	}

	public class NormaGrupo{
		public NormaGrupo(){
			Codigos = new List<string> ();
		}
		public List<string> Codigos{ get; set; }
		public string Titulo{ get; set; }
		public int TotalQ { get; set; }
		public int TotalR { get; set; }
	}

}