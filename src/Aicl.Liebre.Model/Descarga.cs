using ServiceStack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;
using ServiceStack.Model;
using System;


namespace Aicl.Liebre.Model
{
	public class Descarga:IDocument
	{
		public Descarga ()
		{
			Estado = "red";
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string IdDiagnostico { get; set; }
		public DateTime Fecha { get; set; }
		public string Token { get; set; }
		public string Estado { get; set; }
		//public string Responsable { get; set; }
	}

	[Route("/read/descarga","GET")]
	public class ReadDescarga:IReturn<ReadDescargaResponse>
	{
		public string IdDiagnostico { get; set; }
	}

	public class ReadDescargaResponse: IHasResponseStatus 
	{
		public IEnumerable <Descarga> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	[Route("/create/descarga","POST")]
	public class CreateDescarga:IReturn<CreateDescargaResponse>,IHasDataProperty<Descarga>
	{
		public Descarga Data { get; set; }
	}

	public class CreateDescargaResponse: IHasResponseStatus 
	{
		public Descarga Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}


	[Route("/update/descarga","POST")]
	public class UpdateDescarga:IReturn<UpdateDescargaResponse>,IHasDataProperty<Descarga>
	{
		public Descarga Data { get; set; }
	}

	public class UpdateDescargaResponse: IHasResponseStatus 
	{
		public Descarga Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/descarga","POST")]
	public class DeleteDescarga:IReturn<DeleteDescargaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteDescargaResponse: IHasResponseStatus 
	{
		public Descarga Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}
