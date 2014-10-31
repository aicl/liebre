using System;
using Aicl.Liebre.Model;
using ServiceStack.Web;
using System.IO;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{
	public class CIIUService:ServiceBase
	{
		public object Get(ReadCIIU request){
			var f = PathUtils.CombinePaths("~","json","CIIU.json").MapHostAbsolutePath ();
			return 	Store.ReadCIIU (request, f);
		}
	}
}

