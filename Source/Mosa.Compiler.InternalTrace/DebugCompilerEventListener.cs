/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;


namespace Mosa.Compiler.InternalTrace
{
	public class DebugCompilerEventListener : ICompilerEventListener
	{

		void ICompilerEventListener.SubmitTraceEvent(CompilerEvent compilerStage, string info)
		{
			Debug.WriteLine(compilerStage.ToString() + ": " + info);
		}

	}
}
