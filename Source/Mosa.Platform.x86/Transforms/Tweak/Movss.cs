// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Movss
	/// </summary>
	public sealed class Movss : BaseTransformation
	{
		public Movss() : base(X86.Movss, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Result.IsCPURegister)
				return false;

			if (!context.Operand1.IsCPURegister)
				return false;

			return context.Result.Register == context.Operand1.Register;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.Empty();
		}
	}
}
