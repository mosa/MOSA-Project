/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Test.Runtime.CompilerFramework
{
	public class TestCompilerAdapter
	{
		static private TestCompiler compiler = new TestCompiler();

		protected TestCompilerSettings settings = new TestCompilerSettings();

		protected TestCompilerAdapter()
		{
			settings.AddReference("Mosa.Test.Korlib.dll");
		}

		protected T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			CompileTestCode();
			return compiler.Run<T>(settings, ns, type, method, parameters);
		}

		protected void CompileTestCode()
		{
			compiler.CompileTestCode(settings);
		}
	}
}
