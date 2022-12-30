// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Call
	/// </summary>
	public sealed class Call : BaseTransformation
	{
		public Call() : base(X86.Call, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsCPURegister)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			// FIXME: Result operand should be used instead of Operand1 for the result
			// FIXME: Move to FixedRegisterAssignmentStage

			var before = context.InsertBefore();
			var v1 = transform.AllocateVirtualRegister(context.Operand1.Type);

			before.SetInstruction(X86.Mov32, v1, context.Operand1);
			context.Operand1 = v1;
		}
	}
}
