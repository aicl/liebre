using System;

namespace Aicl.Liebre.Data
{
	public interface IHtmlBodyMail
	{
		string RenderToHtml<T> (string template, T model);
	}
}

