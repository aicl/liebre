using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceStack;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Plantilla:IDocument
	{

		public Plantilla ()
		{
		}

		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Version { get; set; }
		public string Titulo { get; set; }
		public DateTime? FechaInicial { get; set; }
		public DateTime? FechaFinal { get; set; }
		//public string Name { get; set; }
	}


	[Route("/read/plantilla","GET")]
	public class ReadPlantilla:IReturn<ReadPlantillaResponse>
	{
	}

	public class ReadPlantillaResponse: IHasResponseStatus 
	{
		public IEnumerable <Plantilla> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}


	[Route("/read/plantilla/{Id}","GET")]
	public class SinglePlantilla:IReturn<SinglePlantillaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class SinglePlantillaResponse
	{
		public Plantilla Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}


	[Route("/create/plantilla","POST")]
	public class CreatePlantilla:IReturn<CreatePlantillaResponse>,IHasDataProperty<Plantilla>
	{
		public Plantilla Data { get; set; }
	}

	public class CreatePlantillaResponse: IHasResponseStatus 
	{
		public Plantilla Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/plantilla","POST")]
	public class UpdatePlantilla:IReturn<UpdatePlantillaResponse>,IHasDataProperty<Plantilla>
	{
		public Plantilla Data { get; set; }
	}

	public class UpdatePlantillaResponse: IHasResponseStatus
	{
		public Plantilla Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/plantilla","POST")]
	public class DeletePlantilla:IReturn<DeletePlantillaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeletePlantillaResponse: IHasResponseStatus 
	{
		public Plantilla Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

}