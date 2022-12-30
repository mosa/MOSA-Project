
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// AddCarryIn32
	/// </summary>
	public sealed class AddCarryIn32 : BaseTransformation
	{
		public AddCarryIn32() : base(IRInstruction.AddCarryIn32, TransformationType.Manual | TransformationType.Transform)
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
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;

			context.SetInstruction(X86.Add32, result, operand1, operand2);
			context.AppendInstruction(X86.Add32, result, result, operand3);
		}
	}
}
