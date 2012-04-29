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
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SimpleRegisterAllocatorStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		#region Data members

		#endregion // Data members

		/// <summary>
		/// Runs the specified method compiler.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.PlugSystem != null)
				if (methodCompiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
					return;

		}

	}
}
