// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// MovLoad16
	/// </summary>
	public sealed class MovLoad16 : BaseTransformation
	{
		public MovLoad16() : base(X86.MovLoad16, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Result.IsCPURegister)
				return false;

			return context.Result.Register == CPURegister.ESI || context.Result.Register == CPURegister.EDI;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovLoad32, result, source, offset);
			context.AppendInstruction(X86.And32, result, result, transform.CreateConstant32(0x0000FFFF));
		}
	}
}
