// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Array
{
	public sealed class CheckArrayBounds : BaseTransform
	{
		public CheckArrayBounds() : base(IRInstruction.CheckArrayBounds, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var arrayOperand = context.Operand1;
			var arrayIndexOperand = context.Operand2;

			// First create new block and split current block
			var newBlocks = transform.CreateNewBlockContexts(1, context.Label);
			var nextBlock = transform.Split(context);

			// Get array length
			var lengthOperand = transform.AllocateVirtualRegister32();

			// Now compare length with index
			// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
			if (transform.Is32BitPlatform)
			{
				context.SetInstruction(IRInstruction.Load32, lengthOperand, arrayOperand, transform.Constant32_0);
				context.AppendInstruction(IRInstruction.Branch32, ConditionCode.UnsignedGreaterOrEqual, null, arrayIndexOperand, lengthOperand, newBlocks[0].Block);
			}
			else
			{
				context.SetInstruction(IRInstruction.Load64, lengthOperand, arrayOperand, transform.Constant64_0);
				context.AppendInstruction(IRInstruction.Branch64, ConditionCode.UnsignedGreaterOrEqual, null, arrayIndexOperand, lengthOperand, newBlocks[0].Block);
			}

			context.AppendInstruction(IRInstruction.Jmp, nextBlock.Block);

			// Build exception block which is just a call to throw exception
			var method = transform.Compiler.InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
			var symbolOperand = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

			newBlocks[0].AppendInstruction(IRInstruction.CallStatic, null, symbolOperand);
		}
	}
}
