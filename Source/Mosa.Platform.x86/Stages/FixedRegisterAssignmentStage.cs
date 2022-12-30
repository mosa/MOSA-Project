// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Transforms.FixedRegisters;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Fixed Register Assignment Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class FixedRegisterAssignmentStage : Compiler.Framework.Stages.BaseTransformationStage
	{
		public override string Name => "x86." + GetType().Name;

		public FixedRegisterAssignmentStage()
			: base(true, false, 1)
		{
			AddTranformations(FixedRegistersTransforms.List);
		}
	}
}
