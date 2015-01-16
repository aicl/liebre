using Aicl.Liebre.Data;
using ServiceStack;
using ServiceStack.Text;
using System;

namespace Aicl.Liebre.ServiceInterface
{

	public class ServiceBase:Service
	{
		public Store Store{ get; set; }

		public AppConfig AppConfig { get; set; }
	
		public IInformant Informant { get; set; }

		public IHtmlBodyMail HtmlBodyMail { get; set; }

		protected static object CreateResponse(object data){
			return new {
				Data=data,
				ResponseStatus= new ResponseStatus()
			};
		}

		public override void PublishMessage<T> (T message){
			try{
				base.PublishMessage(message);
			}
			catch(System.Exception e){
				LogPublishException (message, e, "lbr-local");
				throw new HttpError (e.Message);
			}
		}

		public  void TryPublishMessage<T> (T message){
			try{
				PublishMessage(message);
			}
			catch(HttpError){
			}
		}


		public void PublishMessageToPhantonjs<T>(T request){
			using (var client = new JsonServiceClient (AppConfig.PhantonjsOneWayUrl)) {
				try {
					client.Post (request);
				} catch (Exception e) {
					LogPublishException (request, e, "phn-remoto");
					throw new HttpError (e.Message);
				}
			}
		}

		public void TryPublishMessageToPhantonjs<T>(T request){
			try{
				PublishMessageToPhantonjs(request);
			}
			catch(HttpError){
			}
		}


		public void TrySendMail(Action<OpenShift.Model.Email> mail){
			var m = new OpenShift.Model.SendMail{ ApiKey = AppConfig.PhantonjsApikey };
			mail (m.Email); 
			TryPublishMessageToPhantonjs (m);
		}

		public void SendMail(Action<OpenShift.Model.Email> mail){
			var m = new OpenShift.Model.SendMail{ ApiKey = AppConfig.PhantonjsApikey };
			mail (m.Email); 
			PublishMessageToPhantonjs (m);
		}

		void LogPublishException<T>(T request, System.Exception e, string channel){
			Console.WriteLine ("error en envio de peticion: {0}:{1} {2}", channel, typeof(T), e.Message);
			dynamic message = new {e.Message, Data = request, Type = typeof(T)};
			var serializedMessage = JsonSerializer.SerializeToString((object)message);
			Redis.PushItemToList (channel, serializedMessage);
		}
			
	}
}