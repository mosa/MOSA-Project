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
using System.Text;

using Mosa.Test.CodeDomCompiler;

namespace Mosa.Test.System
{
	public class TestCompilerAdapter
	{
		static private TestCompiler compiler = new TestCompiler();

		protected CompilerSettings settings = new CompilerSettings();

		protected TestCompilerAdapter()
		{
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> f1816ffd8b60479498231c1ffdb6eaa64c555bb6
			settings.AddReference("mscorlib.dll");
			settings.AddReference("Mosa.Platform.x86.Intrinsic.dll");
<<<<<<< HEAD
>>>>>>> 477a0f502b2f82cd1ed0f94fd3c7c2b9f6a2fe7f
=======
			settings.AddReference("Mosa.Test.Runtime.dll");
>>>>>>> f1816ffd8b60479498231c1ffdb6eaa64c555bb6
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
