using System;
using Aicl.Liebre.Model;
using ServiceStack;

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
			var r= Store.CreateRegistroEmpresa (request);
			var url = "{0}/confirmarregistroempresa?Nit={1}&Llave={2}".Fmt(Request.GetBaseUrl(), r.Data.Nit, r.Data.Llave );

			TrySendMail (mail => {
				mail.To.Add(request.Data.Email);
				mail.Subject="SGSST: Solicitud de confirmación";
				mail.Html=HtmlBodyMail.GetHtml(url, typeof(CreateRegistroEmpresa));
			});

			return r;
		}
			
		// esto viene del correo
		public object Get(ConfirmarRegistroEmpresa request){

			var r = Store.ConfirmarRegistroEmpresa (request);
			TrySendMail (mail => {
				mail.To.Add(r.Data.Email);
				mail.Subject="SGSST: Su empresa  ha sido registrada de manera exitosa";
				mail.Html=HtmlBodyMail.GetHtml(r.Data, typeof(ConfirmarRegistroEmpresa));
			});

			return r;
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

