// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.Compound;

public abstract class BaseCompoundTransform : BaseTransform
{
	public BaseCompoundTransform(BaseInstruction instruction, TransformType type, int priority = -10, bool log = false)
		: base(instruction, type, priority, log)
	{ }

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	#region Helpers

	protected static void CopyCompound(Transform transform, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source, Operand operandType)
	{
		var size = transform.MethodCompiler.GetSize(operandType);

		Debug.Assert(size > 0);

		var srcReg = transform.VirtualRegisters.AllocateNativeInteger();
		var dstReg = transform.VirtualRegisters.AllocateNativeInteger();

		context.SetInstruction(IR.UnstableRegionStart);

		context.AppendInstruction(transform.AddInstruction, srcReg, sourceBase, source);
		context.AppendInstruction(transform.AddInstruction, dstReg, destinationBase, destination);

		var tmp = transform.VirtualRegisters.AllocateNativeInteger();

		for (var i = 0; i < size;)
		{
			var left = size - i;

			var index = Operand.CreateConstant32(i);

			if (left >= 8 & !transform.Is32BitPlatform)
			{
				// 64bit move
				context.AppendInstruction(IR.Load64, tmp, srcReg, index);
				context.AppendInstruction(IR.Store64, null, dstReg, index, tmp);
				i += 8;
				continue;
			}
			else if (left >= 4)
			{
				// 32bit move
				context.AppendInstruction(IR.Load32, tmp, srcReg, index);
				context.AppendInstruction(IR.Store32, null, dstReg, index, tmp);
				i += 4;
				continue;
			}
			else if (left >= 2)
			{
				// 16bit move
				context.AppendInstruction(IR.LoadZeroExtend16x32, tmp, srcReg, index);
				context.AppendInstruction(IR.Store16, null, dstReg, index, tmp);
				i += 2;
				continue;
			}
			else
			{
				// 8bit move
				context.AppendInstruction(IR.LoadZeroExtend8x32, tmp, srcReg, index);
				context.AppendInstruction(IR.Store8, null, dstReg, index, tmp);
				i += 1;
				continue;
			}
		}

		context.AppendInstruction(IR.UnstableRegionEnd);
	}

	#endregion Helpers
}
