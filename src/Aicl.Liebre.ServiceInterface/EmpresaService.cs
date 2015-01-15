using System;
using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class EmpresaService:ServiceBase
	{
		// TODO: autorizado y con privilegios de operador ( contiene la llave !);
		public object Get(ReadEmpresa request)
		{
			return ServiceBase.CreateResponse (Store.GetEmpresas (request));
		}

		// TODO: autorizado y con privilegios de operador!
		// TODO: enviar correo informado la llave
		public object Post(CreateEmpresa request)
		{
			return Store.CreateEmpresa (request);
		}


		// TODO: autorizado y con privilegios de operador!
		public object Post(UpdateEmpresa request)
		{
			return Store.UpdateEmpresa (request);
		}

		// TODO: autorizado y con privilegios de operador!
		public object Post(DeleteEmpresa request)
		{
			return Store.DeleteEmpresa (request);
		}

		// Autogestion
		// TODO: enviar correo  para solicitar confirmacion informado la llave
		public object Post(CreateRegistroEmpresa request)
		{
			return Store.CreateRegistroEmpresa (request);
		}


		// esto viene del correo
		public object Post(ConfirmarRegistroEmpresa request){
			return Store.ConfirmarRegistroEmpresa (request);
		}

		public object Post(UpdateRegistroEmpresa request)
		{
			return Store.UpdateRegistroEmpresa (request);
		}

		public object Get(ReadRegistroEmpresa request){
			return ServiceBase.CreateResponse (Store.ReadRegistroEmpresa (request));
		}

		// TODO: enviar correo informado la llave
		public object Post(RecuperarLlaveEmpresa request){
			return Store.RecuperarLlaveEmpresa (request);
		}

	}
}

