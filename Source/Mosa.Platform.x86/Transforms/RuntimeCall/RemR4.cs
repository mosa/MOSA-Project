// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall
{
	/// <summary>
	/// RemR4
	/// </summary>
	public sealed class RemR4 : BaseTransformation
	{
		public RemR4() : base(IRInstruction.RemR4, TransformationType.Manual)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			Debug.Assert(context.Result.IsR4);
			Debug.Assert(context.Operand1.IsR4);

			transformContext.ReplaceWithCall(context, "Mosa.Runtime.Math.x86", "Division", "RemR4");
		}
	}
}
