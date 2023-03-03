// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call
{
	/// <summary>
	/// SetReturnR4
	/// </summary>
	public sealed class SetReturnR4 : BaseTransform
	{
		public SetReturnR4() : base(IRInstruction.SetReturnR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.MoveR4, Operand.CreateCPURegister(context.Operand1.Type, transform.Architecture.ReturnFloatingPointRegister), context.Operand1);
		}
	}
}
