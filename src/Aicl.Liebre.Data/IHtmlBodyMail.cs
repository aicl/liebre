using System;

namespace Aicl.Liebre.Data
{
	public interface IHtmlBodyMail
	{
		string RenderToHtml<T> ( T model, Type request);
	}
}

