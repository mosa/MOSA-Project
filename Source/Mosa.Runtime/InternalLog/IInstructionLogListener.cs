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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.InternalLog
{

	public interface IInstructionLogListener
	{
		void NotifyNewInstructionLog(RuntimeMethod method, string stage, string log);
	}
}
