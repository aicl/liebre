using ServiceStack.Configuration;

namespace Aicl.Liebre.Data
{
	public class AppConfig
	{
		public string PhantonjsURL { get 
			{ return Settings.Get<string> ("PHANTOMJS_URI", "http://localhost:8082"); }
		}

		AppSettings Settings { get; set; }

		public AppConfig (AppSettings settings)
		{
			Settings = settings; 
		}

	}
}