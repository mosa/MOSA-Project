// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

public abstract class BaseIRTransform : BaseARM32Transform
{
	public BaseIRTransform(BaseInstruction instruction, TransformType type, bool log = false)
		: base(instruction, type, log)
	{ }

	#region Overrides

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	#endregion Overrides

	#region Helpers

	//

	#endregion Helpers
}
