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
using System.Diagnostics;

using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.InternalLog
{
	public class DebugCompilerEventListener : ICompilerEventListener
	{

		void ICompilerEventListener.NotifyCompilerEvent(CompilerEvent compilerStage, string info)
		{
			Debug.WriteLine(compilerStage.ToString() + ": " + info);
		}

	}
}
