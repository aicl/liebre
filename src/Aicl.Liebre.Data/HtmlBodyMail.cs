using ServiceStack.IO;
using ServiceStack;
using ServiceStack.Razor;
using ServiceStack.VirtualPath;
using ServiceStack.Testing;
using System.IO;
using System;
using System.Text.RegularExpressions;

namespace Aicl.Liebre.Data
{
	public class HtmlBodyMail:IHtmlBodyMail
	{
		public static readonly string TemplateDir = "MailTemplates";

		static IVirtualPathProvider VirtualPathProvider { 
			get ;
			set;
		}

		RazorFormat Razor { get; set; }

		public HtmlBodyMail ()
		{
			VirtualPathProvider = new FileSystemVirtualPathProvider (new BasicAppHost (),
				Path.Combine ("~".MapHostAbsolutePath (), TemplateDir));

			RazorFormat.Instance = null;
			Razor = new RazorFormat {
				VirtualPathProvider = VirtualPathProvider, 
				EnableLiveReload = false,
				PrecompilePages = true,
				WaitForPrecompilationOnStartup=true,
				//PageBaseType = typeof(ViewPage<>),
			}.Init();
		}

		public string GetHtml<T>( T model, Type requestType){
			var html =Razor.RenderToHtml ("/{0}.cshtml".Fmt (requestType.GetOperationName ()),	model);
			html = Regex.Replace(html, @"\n|\t", " "); 
			html = Regex.Replace(html, @">\s+<", "><").Trim(); 
			return Regex.Replace (html, @"\s{2,}", " ");
		}
	}
}

