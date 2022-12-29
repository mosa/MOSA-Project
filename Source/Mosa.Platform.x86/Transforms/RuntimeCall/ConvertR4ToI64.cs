// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR4ToI64
	/// </summary>
	public sealed class ConvertR4ToI64 : BaseTransformation
	{
		public ConvertR4ToI64() : base(IRInstruction.ConvertR4ToI64, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math", "Conversion", "R4ToI8");
		}
	}
}
