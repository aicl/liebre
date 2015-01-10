using ServiceStack;
using Aicl.Liebre.ServiceInterface;
using Funq;
using Aicl.Liebre.Data;
using ServiceStack.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using System;
using ServiceStack.Logging;
using ServiceStack.Formats;
using ServiceStack.Redis;
using ServiceStack.Messaging.Redis;
using ServiceStack.Messaging;
using Aicl.Liebre.Model;

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

			Plugins.RemoveAll(x => x is HtmlFormat);
			Plugins.Add(new InfoFormat());

			Plugins.Add(new CorsFeature());

			var appConfig = new AppConfig (new  AppSettings ());
			var url = appConfig.MongoURI;
			var conventions = new ConventionPack { new IgnoreExtraElementsConvention(true) };
			ConventionRegistry.Register ("IgnoreExtraElements", conventions, _ => true);

			container.Register<Store> (new Store (url));
			container.Register<IInformant> (new Informant ());
			container.Register<IHtmlBodyMail> (new HtmlBodyMail ());
			container.Register<AppConfig> (appConfig);

			var redisFactory = new BasicRedisClientManager (appConfig.RedisURL);
			container.Register<IRedisClientsManager>(redisFactory); // req. to log exceptions in redis

			var mqServer = new RedisMqServer(container.Resolve<IRedisClientsManager>());
			container.Register<IMessageService>(mqServer);
			container.Register(mqServer.MessageFactory);

			mqServer.RegisterHandler<CreateDiagnosticoInfo> (ServiceController.ExecuteMessage);
			mqServer.Start();

		}
	}
}