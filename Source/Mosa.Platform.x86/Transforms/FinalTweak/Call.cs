// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FinalTweak
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
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Operand1 != null);
			Debug.Assert(context.Operand1.IsCPURegister);

			var before = context.Node.Previous;

			while (before.IsEmpty && !before.IsBlockStartInstruction)
			{
				before = before.Previous;
			}

			if (before == null || before.IsBlockStartInstruction)
				return;

			if (before.Instruction != X86.Mov32)
				return;

			if (!before.Result.IsCPURegister)
				return;

			if (context.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X86.Call, null, before.Operand1);
			context.Empty();
		}
	}
}
