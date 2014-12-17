using Aicl.Liebre.Model;

namespace Aicl.Liebre.ServiceInterface
{
	public class PlanService:ServiceBase
	{
		public object Get(ReadPlan request)
		{
			return ServiceBase.CreateResponse (Store.Get<Plan>(q=>q.Id ));
		}


		public object Post(CreatePlan request)
		{
			return Store.Post<Plan>(request.Data);
		}

		public object Post(UpdatePlan request)
		{
			return Store.Put<Plan>(request.Data);
		}

		public object Post(DeletePlan request)
		{
			return Store.Delete<Plan> (request);
		}
	}
}