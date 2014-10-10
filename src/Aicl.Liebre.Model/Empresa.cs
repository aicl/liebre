﻿using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceStack;
using ServiceStack.Model;

namespace Aicl.Liebre.Model
{

	public class Empresa:IDocument
	{

		public Empresa ()
		{
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id{ get; set; }
		public string Nombre { get; set; }
		public string Nit { get; set; }
		public string Dv { get; set; }
		public string Direccion { get; set; }
		public string Telefono { get; set; }
	}

	[Route("/read/empresa","GET")]
	public class ReadEmpresa:IReturn<ReadEmpresaResponse>
	{
	}

	public class ReadEmpresaResponse: IHasResponseStatus 
	{
		public IEnumerable <Empresa> Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	[Route("/create/empresa","POST")]
	public class CreateEmpresa:IReturn<CreateEmpresaResponse>,IHasDataProperty<Empresa>
	{
		public Empresa Data { get; set; }
	}

	public class CreateEmpresaResponse: IHasResponseStatus 
	{
		public Empresa Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}

	[Route("/update/empresa","POST")]
	public class UpdateEmpresa:IReturn<UpdateEmpresaResponse>,IHasDataProperty<Empresa>
	{
		public Empresa Data { get; set; }
	}

	public class UpdateEmpresaResponse: IHasResponseStatus 
	{
		public Empresa Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}


	[Route("/delete/empresa","POST")]
	public class DeleteEmpresa:IReturn<DeleteEmpresaResponse>, IHasStringId
	{
		public string Id { get; set; }
	}

	public class DeleteEmpresaResponse: IHasResponseStatus 
	{
		public Empresa Data { get; set; }
		public ResponseStatus ResponseStatus {get;set;}
		public WriteResult WriteResult{ get; set; }
	}
}

