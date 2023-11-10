// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.LowerTo32
{
	public abstract class BaseLowerTo32Transform : BaseTransform
	{
		public BaseLowerTo32Transform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		//public override int Priority => 0;

		public override bool Match(Context context, Transform transform)
		{
			return transform.Is32BitPlatform && transform.IsLowerTo32;
		}
	}
}
