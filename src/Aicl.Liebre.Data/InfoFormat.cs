using System;
using ServiceStack;
using ServiceStack.Serialization;
using ServiceStack.Templates;
using ServiceStack.Web;
using System.Linq;
using System.IO;

namespace Aicl.Liebre.Data
{
	public class InfoFormat:IPlugin
	{
		IAppHost AppHost { get; set; }
		public static readonly string InfoDir = "InfoDir";
		public static readonly string InfoFormatFile = "InfoFormat";
		public static readonly string ErrorStatusKey = "__errorStatus";

		public InfoFormat ()
		{
		}

		#region IPlugin implementation
		public void Register (IAppHost appHost)
		{
			AppHost = appHost;
			//Register this in ServiceStack with the custom formats
			appHost.ContentTypes.Register(MimeTypes.Html, SerializeToStream, null);
			appHost.ContentTypes.Register(MimeTypes.JsonReport, SerializeToStream, null);

			appHost.Config.DefaultContentType = MimeTypes.Html;
			appHost.Config.IgnoreFormatsInMetadata.Add(MimeTypes.Html.ToContentFormat());
			appHost.Config.IgnoreFormatsInMetadata.Add(MimeTypes.JsonReport.ToContentFormat());
		}
		#endregion

		public void SerializeToStream(IRequest request, object response, IResponse httpRes)
		{

			var httpResult = request.GetItem("HttpResult") as IHttpResult;
			if (httpResult != null && httpResult.Headers.ContainsKey(HttpHeaders.Location) 
				&& httpResult.StatusCode != System.Net.HttpStatusCode.Created)  
				return;

			try
			{
				if (httpRes.StatusCode >= 400)
				{
					var responseStatus = response.GetResponseStatus();
					request.Items[ErrorStatusKey] = responseStatus;
				}

				if (AppHost.ViewEngines.Any(x => x.ProcessRequest(request, httpRes, response))) return;
			}
			catch (Exception ex)
			{
				if (httpRes.StatusCode < 400)
					throw;

				//If there was an exception trying to render a Error with a View, 
				//It can't handle errors so just write it out here.
				response = DtoUtils.CreateErrorResponse(request.Dto, ex);
			}

			if (request.ResponseContentType != MimeTypes.Html
				&& request.ResponseContentType != MimeTypes.JsonReport) return;

			var dto = response.GetDto();
			var html = dto as string;
			if (html == null)
			{
				// Serialize then escape any potential script tags to avoid XSS when displaying as HTML
				var json = JsonDataContractSerializer.Instance.SerializeToString(dto) ?? "null";
				json = json.Replace("<", "&lt;").Replace(">", "&gt;");

				var url = request.AbsoluteUri
					.Replace("format=html", "")
					.Replace("format=shtm", "")
					.TrimEnd('?', '&');

				url += url.Contains("?") ? "&" : "?";

				var now = DateTime.UtcNow;
				var requestName = request.OperationName ?? dto.GetType().GetOperationName();
				html = GetHtml (request, requestName)
					.Replace ("${Dto}", json)
					.Replace ("${Title}", string.Format (@"{0} - {1}", requestName, now))
					.Replace ("${MvcIncludes}", ServiceStack.MiniProfiler.Profiler.RenderIncludes ().ToString ())
					.Replace ("${Header}", string.Format (@"Información de <i>{0}</i> generada por <a href=""https://servicestack.net"">ServiceStack</a>  a las <b>{1}</b>", requestName, now))
					.Replace("${Humanize}", true.ToString().ToLower())
					.Replace ("${ServiceUrl}", url);
			}

			var utf8Bytes = html.ToUtf8Bytes();
			httpRes.OutputStream.Write(utf8Bytes, 0, utf8Bytes.Length);
		}


		string GetHtml(IRequest request, string requestName){

			var info = request.QueryString["template"]??request.QueryString["Template"]??requestName;

			const string template = "/{0}/{1}.html";

			var file = HostContext.VirtualPathProvider.GetFile(template.Fmt (InfoDir, info)) ??
				HostContext.VirtualPathProvider.GetFile(template.Fmt (InfoDir, requestName)) ??
				HostContext.VirtualPathProvider.GetFile(template.Fmt (InfoDir, InfoFormatFile)) ;
			return (file != null) ? file.ReadAllText() : HtmlTemplates.GetHtmlFormatTemplate ();

		}


		static string LoadTemplate(string templateName)
		{
			var templatePath = "/Templates/" + templateName;
			var file = HostContext.VirtualPathProvider.GetFile(templatePath);
			if (file == null)
				throw new FileNotFoundException("Could not load HTML template embedded resource: " + templatePath, templateName);

			var contents = file.ReadAllText();
			return contents;
		}

	}
}

