using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ServiceStack;
using System.Collections.Generic;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{
	public class Plan:IDocument
	{
		public Plan ()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
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

	[Route("/read/plan","GET")]
	public class ReadPlan:IReturn<ReadPlanResponse>
	{

	}

	public class ReadPlanResponse:IHasResponseStatus
	{
		public IEnumerable<Plan> Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
	}

	[Route("/create/plan","POST")]
	public class CreatePlan:IReturn<CreatePlanResponse>, IHasDataProperty<Plan>
	{
		public Plan Data { get; set; }

	}

	public class CreatePlanResponse:IHasResponseStatus
	{
		public Plan Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/plan","POST")]
	public class UpdatePlan:IReturn<UpdatePlanResponse>, IHasDataProperty<Plan>
	{
		public Plan Data { get; set; }
	}

	public class UpdatePlanResponse:IHasResponseStatus
	{
		public Plan Data { get; set; }
		public ResponseStatus ResponseStatus { get; set; }
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/delete/plan","POST")]
	public class DeletePlan:IReturn<DeletePlanResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeletePlanResponse: IHasResponseStatus 
	{
		public Plan Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}
}

