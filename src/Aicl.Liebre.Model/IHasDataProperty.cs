
using ServiceStack;

namespace Aicl.Liebre.Model
{
	public interface IHasDataProperty<T> where T: IDocument
	{
		T Data {get;set;}
	}


	public class WriteResult
	{
		public WriteResult(){}
		public long DocumentsAffected { get; set; }
		public string CommandName { get; set; }
		public string ErrorMessage { get; set; }
		public bool Ok { get; set; }
	}

	public class Result<T>:IHasResponseStatus, IHasDataProperty<T> where T:IDocument
	{

		public Result (){
			ResponseStatus = new ResponseStatus ();
	
		}

		public WriteResult WriteResult { get; set; }

		#region IHasDataProperty implementation
		public T Data { get; set; }
		#endregion

		#region IHasResponseStatus implementation

		public ResponseStatus ResponseStatus { get; set; }

		#endregion
	}
}

