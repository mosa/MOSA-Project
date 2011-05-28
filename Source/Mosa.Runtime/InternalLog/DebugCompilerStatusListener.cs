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
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.InternalLog
{
	public class DebugCompilerStatusListener : ICompilerStatusListener
	{

		void ICompilerStatusListener.NotifyCompilerStatus(ICompilerStage compilerStage, string info)
		{
			Debug.WriteLine(compilerStage.ToString() + ": " + info);
		}

	}
}
