// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.Compound;

public abstract class BaseCompoundTransform : BaseTransform
{
	public BaseCompoundTransform(BaseInstruction instruction, TransformType type, bool log = false)
		: base(instruction, type, log)
	{ }

	#region Helpers

	protected static void CopyCompound(TransformContext transform, Context context, Operand destinationBase, Operand destination, Operand sourceBase, Operand source)
	{
		var size = transform.MethodCompiler.GetSize(source);

		Debug.Assert(size > 0);

		var srcReg = transform.VirtualRegisters.AllocateNativeInteger();
		var dstReg = transform.VirtualRegisters.AllocateNativeInteger();

		context.SetInstruction(IRInstruction.UnstableObjectTracking);

		context.AppendInstruction(transform.AddInstruction, srcReg, sourceBase, source);
		context.AppendInstruction(transform.AddInstruction, dstReg, destinationBase, destination);

		var tmp = transform.VirtualRegisters.Allocate32();
		var tmpLarge = transform.Is32BitPlatform && size >= 8 ? null : transform.VirtualRegisters.Allocate64();

		for (var i = 0; i < size;)
		{
			var left = size - i;

			var index = transform.CreateConstant32(i);

			if (left >= 8 & !transform.Is32BitPlatform)
			{
				// 64bit move
				context.AppendInstruction(IRInstruction.Load64, tmpLarge, srcReg, index);
				context.AppendInstruction(IRInstruction.Store64, null, dstReg, index, tmpLarge);
				i += 8;
				continue;
			}
			else if (left >= 4)
			{
				// 32bit move
				context.AppendInstruction(IRInstruction.Load32, tmp, srcReg, index);
				context.AppendInstruction(IRInstruction.Store32, null, dstReg, index, tmp);
				i += 4;
				continue;
			}
			else if (left >= 2)
			{
				// 16bit move
				context.AppendInstruction(IRInstruction.LoadParamZeroExtend16x32, tmp, srcReg, index);
				context.AppendInstruction(IRInstruction.Store16, null, dstReg, index, tmp);
				i += 2;
				continue;
			}
			else
			{
				// 8bit move
				context.AppendInstruction(IRInstruction.LoadParamZeroExtend8x32, tmp, srcReg, index);
				context.AppendInstruction(IRInstruction.Store8, null, dstReg, index, tmp);
				i += 1;
				continue;
			}
		}

		context.AppendInstruction(IRInstruction.StableObjectTracking);
	}

	#endregion Helpers
}
