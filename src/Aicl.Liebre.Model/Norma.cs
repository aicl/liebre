namespace Aicl.Liebre.Model
{
	public class Norma:IDocument
	{
		public Norma ()
		{
		}
		public string Id { get { return Codigo; } set{ Codigo = value; } }
		public string Codigo { get; set; }
		public string Nombre { get; set; }
		public string Anotacion { get; set; }
		public string Titulo { get; set; }
		public string Contenido { get; set; }
		public string Alias { get; set; }

	}
}