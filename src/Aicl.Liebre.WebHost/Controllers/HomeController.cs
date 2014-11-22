using System.Web.Mvc;
using ServiceStack.Mvc;

namespace Aicl.Liebre.WebHost
{
	public class HomeController : ServiceStackController
	{
		public ActionResult Index ()
		{
			return Redirect ("/");
		}
	}
}

