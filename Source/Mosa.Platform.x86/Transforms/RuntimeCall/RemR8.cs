// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// RemR8
	/// </summary>
	public sealed class RemR8 : BaseTransformation
	{
		public RemR8() : base(IRInstruction.RemR8, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			Debug.Assert(context.Result.IsR8);
			Debug.Assert(context.Operand1.IsR8);

			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math.x86", "Division", "RemR8");
		}
	}
}
