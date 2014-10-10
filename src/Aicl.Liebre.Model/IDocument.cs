using ServiceStack.Model;

namespace Aicl.Liebre.Model
{
	public interface IDocument:IHasStringId
	{
		new string Id { get; set; }
	}
}

