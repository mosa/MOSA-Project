// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call
{
	/// <summary>
	/// SetReturnR8
	/// </summary>
	public sealed class SetReturnR8 : BaseTransform
	{
		public SetReturnR8() : base(IRInstruction.SetReturnR8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.MoveR8, Operand.CreateCPURegister(context.Operand1.Type, transform.Architecture.ReturnFloatingPointRegister), context.Operand1);
		}
	}
}
