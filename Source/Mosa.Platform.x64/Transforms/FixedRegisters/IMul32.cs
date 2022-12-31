using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// IMul32
	/// </summary>
	public sealed class IMul32 : BaseTransform
	{
		public IMul32() : base(X64.IMul32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Operand2.IsConstant;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var v1 = transform.AllocateVirtualRegister(context.Operand2.Type);
			var operand2 = context.Operand2;

			context.Operand2 = v1;
			context.InsertBefore().SetInstruction(X64.Mov64, v1, operand2);
		}
	}
}
