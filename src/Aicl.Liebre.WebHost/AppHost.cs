using ServiceStack;
using Aicl.Liebre.ServiceInterface;
using Funq;
using Aicl.Liebre.Data;
using ServiceStack.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using System.Web.Mvc;
using ServiceStack.Mvc;
using System;
using ServiceStack.Logging;

namespace Aicl.Liebre.WebHost
{
	public class AppHost:AppHostBase
	{
		public AppHost ():base("Liebre SGSST", typeof(ServiceBase).Assembly)
		{
		}

		public override void Configure(Container container){

			LogManager.LogFactory = new ConsoleLogFactory();

			SetConfig(new HostConfig
				{
					DebugMode = true,
					HandlerFactoryPath = "lbr-api",
				});

			Plugins.Add(new CorsFeature());
			var appSettings = new AppSettings();
			var url = appSettings.Get("MONGOLAB_URI", appSettings.Get("MONGOTEST_URI") );
			var conventions = new ConventionPack { new IgnoreExtraElementsConvention(true) };
			ConventionRegistry.Register ("IgnoreExtraElements", conventions, _ => true);
			container.Register<Store> (new Store (url));
			ControllerBuilder.Current.SetControllerFactory (
				new FunqControllerFactory (container));
		}
	}
}