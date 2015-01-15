
using ServiceStack;
using System.Collections.Generic;

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

	public class BulkWrite{
		public BulkWrite(){}
		public long DeleteCount { get; set;}
		public long InsertedCount { get; set;}
		public long MatchedCount { get; set;}
		public long UpsertsCount { get; set;}

	}

	public class Result<T>:IHasResponseStatus, IHasDataProperty<T> where T:IDocument
	{

		public Result (){
			ResponseStatus = new ResponseStatus ();
			WriteResult = new WriteResult{ Ok = true };
		}

		public WriteResult WriteResult { get; set; }

		#region IHasDataProperty implementation
		public T Data { get; set; }
		#endregion

		#region IHasResponseStatus implementation
		public ResponseStatus ResponseStatus { get; set; }
		#endregion
	}

	//

	public class BulkResult<T>:IHasResponseStatus, IHasDataProperty<T> where T:IDocument
	{

		public BulkResult (){
			ResponseStatus = new ResponseStatus ();
		}

		public BulkWrite BulkWrite { get; set; }

		#region IHasDataProperty implementation
		public T Data { get; set; }
		#endregion

		#region IHasResponseStatus implementation
		public ResponseStatus ResponseStatus { get; set; }
		#endregion
	}


	public class ListResult<T>: IHasResponseStatus 
	{
		long? totalCount;

		public ListResult(){
			Data = new List<T> ();
		}

		public List<T> Data {get;set;}
		public long? TotalCount {
			get {return totalCount.HasValue? totalCount.Value: Data.Count;}
			set { totalCount=value;}
		}
		public ResponseStatus ResponseStatus { get; set; }
	}

}

