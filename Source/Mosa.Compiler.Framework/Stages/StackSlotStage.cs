// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
