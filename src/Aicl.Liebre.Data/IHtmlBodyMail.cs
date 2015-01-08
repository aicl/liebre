using System;

namespace Aicl.Liebre.Data
{
	public interface IHtmlBodyMail
	{
		string GetHtml<T> ( T model, Type request);
	}
}

