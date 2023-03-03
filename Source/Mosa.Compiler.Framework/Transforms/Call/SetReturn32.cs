// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call
{
	/// <summary>
	/// SetReturn32
	/// </summary>
	public sealed class SetReturn32 : BaseTransform
	{
		public SetReturn32() : base(IRInstruction.SetReturn32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(IRInstruction.Move32, Operand.CreateCPURegister(context.Operand1.Type, transform.Architecture.ReturnRegister), context.Operand1);
		}
	}
}
