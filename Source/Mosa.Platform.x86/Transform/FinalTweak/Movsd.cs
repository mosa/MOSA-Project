// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.FinalTweak
{
	/// <summary>
	/// Movsd
	/// </summary>
	public sealed class Movsd : BaseTransformation
	{
		public Movsd() : base(X86.Movsd, TransformationType.Manual)
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
