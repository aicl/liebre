using System;
using System.IO;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.IO;
using System.Linq;
using ServiceStack.Serialization;

namespace Aicl.Liebre.Data
{
	public class Informant:IInformant
	{
		string RepoPath { get; set; }

		static IVirtualPathProvider VirtualPathProvider { 
			get { return HostContext.VirtualPathProvider; } 
		}

		public Informant ():this( InfoFormat.InfoDir){
		}

		public Informant ( string repoPath)
		{
			RepoPath = repoPath;

		}

		public IEnumerable<IVirtualFile> GetAllFileInfo<T>(){
			var operation = typeof (T).GetOperationName ();

			IEnumerable<IVirtualFile> files = null;

			var dir = VirtualPathProvider.GetDirectory ("{0}/{1}".Fmt (RepoPath, operation));
			if (dir != null) {
				files = dir.GetAllMatchingFiles ("*.html");
			}

			return (files!=null && files.Any ()) ?
				files :
				VirtualPathProvider.GetDirectory (RepoPath).GetAllMatchingFiles ("{0}.html".Fmt (operation));

		}

		public string GetHtml<T>(T response, IVirtualFile vf){
			var json = JsonDataContractSerializer.Instance.SerializeToString(response) ?? "null";
			json = json.Replace("<", "&lt;").Replace(">", "&gt;");
			return vf.ReadAllText ().Replace ("${Dto}", json);
		}

		public byte[] GetUtf8Bytes<T>(T response, IVirtualFile vf){
			return GetHtml<T> (response, vf).ToUtf8Bytes ();
		}

	}
}
