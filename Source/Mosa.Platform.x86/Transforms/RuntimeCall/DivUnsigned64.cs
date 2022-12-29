// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// DivUnsigned64
	/// </summary>
	public sealed class DivUnsigned64 : BaseTransformation
	{
		public DivUnsigned64() : base(IRInstruction.DivUnsigned64, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "udiv64");
		}
	}
}
