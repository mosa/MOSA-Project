// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR
{
	public abstract class BaseIRTransform : BaseX86Transform
	{
		public BaseIRTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Overrides

		public override bool Match(Context context, Transform transform)
		{
			return true;
		}

		#endregion Overrides
	}
}
