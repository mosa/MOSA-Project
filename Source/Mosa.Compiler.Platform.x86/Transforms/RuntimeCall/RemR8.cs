// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// RemR8
	/// </summary>
	public sealed class RemR8 : BaseTransform
	{
		public RemR8() : base(IRInstruction.RemR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.x86.Division", "RemR8");
		}
	}
}
