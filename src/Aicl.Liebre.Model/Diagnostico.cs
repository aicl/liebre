using System;
using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Diagnostico:IDocument
	{
		public Diagnostico ()
		{
			Descargas = new List<Descarga> ();
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id{ get; set; }
		public string IdEmpresa { get; set; }
		public string IdPlantilla { get; set; }
		public DateTime Creado { get; set; }
		public string Descripcion { get; set; }
		public int Revision{ get; set; }
		//public DateTime? FechaInicio { get; set; }
		[BsonIgnore]
		public List<Descarga> Descargas { get; set; }


	}
	[Route("/read/diagnostico","GET")]
	public class ReadDiagnostico:IReturn<ReadDiagnosticoResponse>
	{
		public string Id{ get; set; }
		public string IdEmpresa { get; set; }
		public string IdPlantilla { get; set; }
	}

	public class ReadDiagnosticoResponse:IHasResponseStatus
	{
		public IEnumerable<Diagnostico> Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
	}

	[Route("/create/diagnostico","POST")]
	public class CreateDiagnostico:IReturn<CreateDiagnosticoResponse>, IHasDataProperty<Diagnostico>
	{
		public Diagnostico Data { get; set; }

	}

	public class CreateDiagnosticoResponse:IHasResponseStatus
	{
		public Diagnostico Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/diagnostico","POST")]
	public class UpdateDiagnostico:IReturn<UpdateDiagnosticoResponse>, IHasDataProperty<Diagnostico>
	{
		public Diagnostico Data { get; set; }
	}

	public class UpdateDiagnosticoResponse:IHasResponseStatus
	{
		public Diagnostico Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/diagnostico","POST")]
	public class DeleteDiagnostico:IReturn<DeleteDiagnosticoResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteDiagnosticoResponse: IHasResponseStatus 
	{
		public Diagnostico Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}
