// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR4ToU64
	/// </summary>
	public sealed class ConvertR4ToU64 : BaseTransformation
	{
		public ConvertR4ToU64() : base(IRInstruction.ConvertR4ToU64, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math", "Conversion", "R4ToU8");
		}
	}
}
