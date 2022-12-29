// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// DivSigned64
	/// </summary>
	public sealed class DivSigned64 : BaseTransformation
	{
		public DivSigned64() : base(IRInstruction.DivSigned64, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math", "Division", "sdiv64");
		}
	}
}
