using Aicl.Liebre.Data;
using ServiceStack;
using ServiceStack.Text;

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
				} catch (System.Exception e) {
					LogPublishException (request, e, "phn-remoto");
					throw new HttpError (e.Message);
				}
			}
		}

		void LogPublishException<T>(T request, System.Exception e, string channel){
			System.Console.WriteLine ("error en envio de peticion: {0}:{1} {2}", channel, typeof(T), e.Message);
			dynamic message = new {Message=e.Message, Data= request};
			var serializedMessage = JsonSerializer.SerializeToString((object)message);
			Redis.PublishMessage (channel, serializedMessage);
		}
	}
}