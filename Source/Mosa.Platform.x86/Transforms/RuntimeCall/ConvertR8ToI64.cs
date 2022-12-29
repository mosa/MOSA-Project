// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// ConvertR8ToI64
	/// </summary>
	public sealed class ConvertR8ToI64 : BaseTransformation
	{
		public ConvertR8ToI64() : base(IRInstruction.ConvertR8ToI64, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math", "Conversion", "R8ToI8");
		}
	}
}
