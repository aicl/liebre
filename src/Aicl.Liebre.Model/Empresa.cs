using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ServiceStack;
using ServiceStack.Model;
using System;

namespace Aicl.Liebre.Model
{

	public class Empresa:IDocument
	{

		public Empresa ()
		{
			Plan = new Plan ();
		}
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id{ get; set; }
		public string Nombre { get; set; }
		public string Nit { get; set; }
		public string Direccion { get; set; }
		public string Telefono { get; set; }
		public string Email { get; set; }
		public string Contacto { get; set; }
		public string IdPlan { get; set; }
		public string Llave { get; set; }
		[BsonIgnore]
		public Plan Plan {get;set;}
		[ReadOnly]
		public virtual DateTime? FechaRegistro{ get; set;}
		[ReadOnly]
		public virtual DateTime FechaLLave{ get; set;}
		// Saldo : cuantos le quedaron del plan anterior ?
	}

	[Collection(typeof(Empresa))]
	public class EmpresaFechaRegistro:IDocument
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id{ get; set; }
		public  DateTime? FechaRegistro { get; set;}
	}


	public class EmpresaLogo:Empresa{

		public string LogoType { get; set; }
		public string Logo { get; set; }
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

	//Autogestion

	[Route("/createregistroempresa","POST")]
	public class CreateRegistroEmpresa:IReturn<CreateRegistroEmpresaResponse>,IHasDataProperty<Empresa>
	{
		public Empresa Data { get; set; }
	}


	[Route("/updateregistroempresa","POST")]
	public class UpdateRegistroEmpresa:IReturn<UpdateRegistroEmpresaResponse>,IHasDataProperty<Empresa>
	{
		public Empresa Data { get; set; }
	}

	[Route("/readregistroempresa","GET")]
	public class ReadRegistroEmpresa:IReturn<ReadRegistroEmpresaResponse>
	{
		public string Nit { get; set; }
		public string Llave { get; set; }

	}

	[Route("/confirmarregistroempresa","GET")]
	public class ConfirmarRegistroEmpresa:IReturn<ConfirmarRegistroEmpresaResponse>
	{
		public string Nit { get; set; }
		public string Llave { get; set; }
	}

	[Route("/recuperarllaveempresa","POST")]
	public class RecuperarLlaveEmpresa:IReturn<RecuperarLlaveEmpresaResponse>
	{
		public string Nit { get; set; }
		public string Llave { get; set; }
		public bool Regenerar { get; set; }
	}

	public class CreateRegistroEmpresaResponse:CreateEmpresaResponse {
	}

	public class UpdateRegistroEmpresaResponse:UpdateEmpresaResponse{
	}


	public class ReadRegistroEmpresaResponse:IHasResponseStatus
	{
		public Empresa Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	public class RecuperarLlaveEmpresaResponse:IHasResponseStatus
	{
		public Empresa Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

	public class ConfirmarRegistroEmpresaResponse:IHasResponseStatus
	{
		public Empresa Data {get;set;}
		public ResponseStatus ResponseStatus {get;set;}
	}

}

