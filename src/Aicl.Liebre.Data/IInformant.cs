using System;
using ServiceStack.IO;
using System.Collections.Generic;

namespace Aicl.Liebre.Data
{
	public interface IInformant
	{
		IEnumerable<IVirtualFile> GetAllFileInfo<T> ();
		string GetHtml<T> (T request, IVirtualFile vf);
	}
}

