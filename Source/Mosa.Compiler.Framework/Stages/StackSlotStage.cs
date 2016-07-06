// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Calculates the layout of the stack of the method.
	/// </summary>
	public sealed class StackSlotStage : BaseMethodCompilerStage
	{
		protected override void Run()
		{
			if (IsPlugged)
				return;
		}
	}
}
