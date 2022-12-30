// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;
using System.Diagnostics;

namespace Mosa.Platform.x86.Transforms.FinalTweak
{
	/// <summary>
	/// Mov32
	/// </summary>
	public sealed class Mov32 : BaseTransformation
	{
		public Mov32() : base(X86.Mov32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Operand1.IsCPURegister && context.Result.Register == context.Operand1.Register;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);

			context.Empty();
		}
	}
}
