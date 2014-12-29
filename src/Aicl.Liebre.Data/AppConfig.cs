using ServiceStack.Configuration;
using ServiceStack;

namespace Aicl.Liebre.Data
{
	public class AppConfig
	{
		public string PhantonjsURL {
			get	{ return Settings.Get<string> ("PHANTOMJS_URI", "http://localhost:8082"); }
		}

		public string PhantonjsApiString{
			get { return "phn-api"; }
		}


		public string PhantonjApiUrl{
			get{ return "{0}/{1}".Fmt (PhantonjsURL, PhantonjsApiString); }
		}

		public string PhantonjsOneWayUrl{
			get { return "{0}/json/oneway".Fmt (PhantonjApiUrl); }
		}

		public string PhantonjsReplyUrl{
			get { return "{0}/json/reply".Fmt (PhantonjApiUrl); }
		}

		AppSettings Settings { get; set; }

		public AppConfig (AppSettings settings)
		{
			Settings = settings; 
		}

	}
}