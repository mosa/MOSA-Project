// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.BaseIR
{
	public abstract class BaseIRTransform : BaseX64Transform
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

		#region Helpers

		//

		#endregion Helpers
	}
}
