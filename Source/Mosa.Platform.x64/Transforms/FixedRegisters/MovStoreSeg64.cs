using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// MovStoreSeg64
	/// </summary>
	public sealed class MovStoreSeg64 : BaseTransform
	{
		public MovStoreSeg64() : base(X64.MovStoreSeg64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Operand1.IsConstant;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var v1 = transform.AllocateVirtualRegister(operand1.Type);

			context.SetInstruction(X64.Mov64, v1, operand1);
			context.AppendInstruction(X64.MovStoreSeg64, result, v1);
		}
	}
}
