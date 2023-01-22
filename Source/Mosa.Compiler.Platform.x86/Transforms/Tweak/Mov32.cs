// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// Mov32
	/// </summary>
	public sealed class Mov32 : BaseTransform
	{
		public Mov32() : base(X86.Mov32, TransformType.Manual | TransformType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Result.IsCPURegister && context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);

			context.Empty();
		}
	}
}
