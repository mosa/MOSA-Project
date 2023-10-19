// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

public sealed class CombineAddressOfStore32 : BaseTransform
{
	public CombineAddressOfStore32() : base(IRInstruction.Store32, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned32 != 4)
			return false;

		if (!context.Operand3.HasParent)
			return false;

		if (!context.Operand1.IsDefinedOnce)
			return false;

		if (!context.Operand3.IsDefinedOnce)
			return false;

		var addressof = context.Operand1.Definitions[0];

		if (addressof.Instruction != IRInstruction.AddressOf)
			return false;

		if (!addressof.Operand1.IsLocalStack)
			return false;

		var store2 = context.Node.PreviousNonEmpty;

		if (store2.IsBlockStartInstruction)
			return false;

		if (store2.Instruction != IRInstruction.Store32)
			return false;

		if (store2.Operand1 != transform.StackFrame)
			return false;

		if (!store2.Operand3.IsDefinedOnce)
			return false;

		if (store2.Operand3.Parent != context.Operand3.Parent)
			return false;

		if (store2.Operand3 == context.Operand3)
			return false;

		if (store2.Operand2 != addressof.Operand1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var addressof = context.Operand1.Definitions[0];
		var store2 = context.Node.PreviousNonEmpty;

		context.SetInstruction(IRInstruction.Store64, null, transform.StackFrame, addressof.Operand1, store2.Operand3.Parent);
		store2.Empty();
	}
}
