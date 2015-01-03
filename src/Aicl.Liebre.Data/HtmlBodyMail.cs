using ServiceStack.IO;
using ServiceStack;
using ServiceStack.Razor;
using ServiceStack.VirtualPath;
using ServiceStack.Testing;
using System.IO;

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

		public string RenderToHtml<T>(string template, T model){
			return Razor.RenderToHtml ("/{0}.cshtml".Fmt (template), model);
		}
	}
}

