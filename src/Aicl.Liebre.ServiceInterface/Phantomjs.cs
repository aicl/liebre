using System;
using System.Diagnostics;
using System.IO;
using ServiceStack;

namespace Aicl.Liebre.ServiceInterface
{
	public class Phantomjs
	{
		string Exe { get; set; }
		string Rasterize { get; set; }

		public Phantomjs(){
			Exe = PathUtils.CombinePaths("~","phantomjs","phantomjs").MapHostAbsolutePath ();
			Rasterize = PathUtils.CombinePaths("~","phantomjs","rasterize.js").MapHostAbsolutePath ();
		}

		public int Execute( string url, string outputfile)
		{
			var args = "{0} {1} {2} {3}".Fmt (Rasterize, url, outputfile, "Letter");
			var oInfo = new ProcessStartInfo(Exe, args);
			oInfo.UseShellExecute = false; 
			oInfo.CreateNoWindow = true;

			oInfo.RedirectStandardOutput = true;
			oInfo.RedirectStandardError = true;

			using (Process proc = Process.Start (oInfo)) {
				proc.WaitForExit ();
				using (var srOutput = proc.StandardOutput) {
					StandardOutput = srOutput.ReadToEnd ();
				}
				using (var srError = proc.StandardError) {
					StandardError = srError.ReadToEnd ();
				}
				int exitCode = proc.ExitCode;
				proc.Close ();
				return exitCode;
			}
		}

		public string StandardOutput
		{
			get;
			private set;
		}
		public string StandardError {
			get;
			private set;
		}

	}
}

