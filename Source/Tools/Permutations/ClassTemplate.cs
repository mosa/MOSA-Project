using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public class ClassTemplate
	{

		public static IList<string> AddClass(string className, IList<string> lines)
		{
			List<string> newlines = new List<string>(lines.Count + 50);

			newlines.Add("/*");
			newlines.Add(" * (c) 2010 MOSA - The Managed Operating System Alliance");
			newlines.Add(" *");
			newlines.Add(" * Licensed under the terms of the New BSD License.");
			newlines.Add(" *");
			newlines.Add(" * Authors:");
			newlines.Add(" *  Phil Garcia (tgiphil) <phil@thinkedge.com>");
			newlines.Add(" */");
			newlines.Add("");
			newlines.Add("/* DO NOT MODIFY THIS FILE COMPUTER GENERATED CODE. */");
			newlines.Add("");
			newlines.Add("using System;");
			newlines.Add("");
			newlines.Add("using MbUnit.Framework;");
			newlines.Add("");
			newlines.Add("namespace Test.Mosa.Runtime.CompilerFramework.CLI");
			newlines.Add("{");
			newlines.Add("public partial class " + className);
			newlines.Add("{");

			foreach (string line in lines)
				newlines.Add(line);

			newlines.Add("}");
			newlines.Add("}");

			return newlines;
		}
	}
}
