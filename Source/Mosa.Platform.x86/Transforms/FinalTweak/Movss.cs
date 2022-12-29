// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FinalTweak
{
	/// <summary>
	/// Movss
	/// </summary>
	public sealed class Movss : BaseTransformation
	{
		public Movss() : base(X86.Movss, TransformationType.Manual | TransformationType.Tranformation)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Result.Register == context.Operand1.Register;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Operand1.IsCPURegister);

			context.Empty();
		}
	}
}
