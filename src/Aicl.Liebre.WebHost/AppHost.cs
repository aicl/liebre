using ServiceStack;
using Aicl.Liebre.ServiceInterface;
using Funq;
using Aicl.Liebre.Data;
using ServiceStack.Configuration;
using MongoDB.Bson.Serialization.Conventions;


namespace Aicl.Liebre.WebHost
{
	public class AppHost:AppHostBase
	{
		public AppHost ():base("Liebre SGSST", typeof(ServiceBase).Assembly)
		{
		}

		public override void Configure(Container container){
			//Plugins.Add(new CorsFeature());
			base.SetConfig(new HostConfig
				{
					GlobalResponseHeaders = {
						{ "Access-Control-Allow-Origin", "*" },
						{ "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
						{ "Access-Control-Allow-Headers", "Content-Type" },
					},
					DefaultContentType = "application/json"
				});

			var appSettings = new AppSettings();

			var url = appSettings.Get("MONGOLAB_URI", appSettings.Get("MONGOTEST_URI") );

			var conventions = new ConventionPack { new IgnoreExtraElementsConvention(true) };

			ConventionRegistry.Register ("IgnoreExtraElements", conventions, _ => true);

			container.Register<Store> (new Store (url));
		}

	}
}