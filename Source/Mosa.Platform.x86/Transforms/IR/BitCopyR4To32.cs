
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// BitCopyR4To32
	/// </summary>
	public sealed class BitCopyR4To32 : BaseTransformation
	{
		public BitCopyR4To32() : base(IRInstruction.BitCopyR4To32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = X86TransformHelper.MoveConstantToFloatRegister(context, operand1, transform);

			context.SetInstruction(X86.Movdssi32, result, operand1);
		}
	}
}
