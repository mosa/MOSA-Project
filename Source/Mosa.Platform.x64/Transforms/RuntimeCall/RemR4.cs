// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.RuntimeCall
{
	/// <summary>
	/// RemR4
	/// </summary>
	public sealed class RemR4 : BaseTransform
	{
		public RemR4() : base(IRInstruction.RemR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.x64", "Division", "RemR4");
		}
	}
}
