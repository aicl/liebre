using System;

namespace Aicl.Liebre.Model
{
	public class Plan
	{
		public Plan ()
		{
		}

		public string Id{ get; set; }
		public string Nombre { get; set; }
		public DateTime? Desde { get; set; }
		public DateTime? Hasta { get; set; }
		public string Commetario { get; set; }
		public int Precio { get; set; }
		public string IdPlantilla  { get; set; }
		public bool Demo { get; set; }
		public bool Aprobado { get; set; }
	}
}

