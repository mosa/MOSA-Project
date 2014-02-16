/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using Mosa.Test.CodeDomCompiler;

namespace Mosa.Test.System
{
	public class TestCompilerAdapter
	{
		private static TestCompiler compiler = new TestCompiler();

		protected CompilerSettings settings = new CompilerSettings();

		protected TestCompilerAdapter()
		{
			settings.AddReference("mscorlib.dll");
			settings.AddReference("Mosa.Platform.Internal.x86.dll");
			settings.AddReference("Mosa.Kernel.x86Test.dll");
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