using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Tools.TypeExplorer
{
	class StringLogger : ILoggerWriter
	{
		List<string> sb;
		int indentlength = 0;
		public StringLogger()
		{
			sb = new List<string>();
		}
		public void WriteLine(string s)
		{
			sb.Add(new string(' ', indentlength) + s);
		}
		public void Indent()
		{
			indentlength += 4;
		}
		public void Unindent()
		{
			indentlength -= 4;
		}
		public string[] GetFullLog()
		{
			return sb.ToArray();
		}
	}
}
