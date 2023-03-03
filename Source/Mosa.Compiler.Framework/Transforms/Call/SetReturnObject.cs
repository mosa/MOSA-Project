// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call
{
	/// <summary>
	/// SetReturnObject
	/// </summary>
	public sealed class SetReturnObject : BaseTransform
	{
		public SetReturnObject() : base(IRInstruction.SetReturnObject, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.MoveObject, Operand.CreateCPURegister(context.Operand1.Type, transform.Architecture.ReturnRegister), context.Operand1);
		}
	}
}
