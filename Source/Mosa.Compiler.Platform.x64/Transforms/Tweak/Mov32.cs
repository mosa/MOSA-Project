// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Mov32
	/// </summary>
	public sealed class Mov32 : BaseTransform
	{
		public Mov32() : base(X64.Mov32, TransformType.Manual | TransformType.Transform)
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
