// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;
using System.Diagnostics;

namespace Mosa.Platform.x86.Transforms.FinalTweak
{
	/// <summary>
	/// Movsd
	/// </summary>
	public sealed class Movsd : BaseTransformation
	{
		public Movsd() : base(X86.Movsd, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Result.Register == context.Operand1.Register;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Operand1.IsCPURegister);

			context.Empty();
		}
	}
}
