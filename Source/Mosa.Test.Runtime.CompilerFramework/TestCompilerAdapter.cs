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

		protected T Run<T>(string ns, string type, string method, params object[] parameters)
		{
			settings.AdditionalSource = Code.AllTestCode;
			return compiler.Run<T>(settings, ns, type, method, parameters);
		}

		protected void CompileTestCode()
		{
			settings.AdditionalSource = Code.AllTestCode;
			compiler.CompileTestCode(settings);
		}
	}
}
