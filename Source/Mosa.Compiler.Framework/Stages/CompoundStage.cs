// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Transforms compound instructions into lower instructions
	/// </summary>
	public sealed class CompoundStage : BaseCodeTransformationStageLegacy
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.LoadCompound, LoadCompound);
			AddVisitation(IRInstruction.LoadParamCompound, LoadParamCompound);
			AddVisitation(IRInstruction.MoveCompound, MoveCompound);
			AddVisitation(IRInstruction.StoreCompound, StoreCompound);
			AddVisitation(IRInstruction.StoreParamCompound, StoreParamCompound);
		}

		private void StoreCompound(Context context)
		{
			CopyCompound(context, context.Operand3.Type, context.Operand1, context.Operand2, StackFrame, context.Operand3);
		}

		private void StoreParamCompound(Context context)
		{
			CopyCompound(context, context.Operand2.Type, StackFrame, context.Operand1, StackFrame, context.Operand2);
		}

		private void LoadCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, context.Operand1, context.Operand2);
		}

		private void MoveCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void LoadParamCompound(Context context)
		{
			CopyCompound(context, context.Result.Type, StackFrame, context.Result, StackFrame, context.Operand1);
		}

		private void CopyCompound(Context context, MosaType type, Operand destinationBase, Operand destination, Operand sourceBase, Operand source)
		{
			int size = TypeLayout.GetTypeSize(type);

			Debug.Assert(size > 0);

			var addInstruction = Is32BitPlatform ? (BaseInstruction)IRInstruction.Add32 : IRInstruction.Add64;

			var srcReg = AllocateVirtualRegister(Is32BitPlatform ? destinationBase.Type.TypeSystem.BuiltIn.I4 : destinationBase.Type.TypeSystem.BuiltIn.I8);
			var dstReg = AllocateVirtualRegister(Is32BitPlatform ? destinationBase.Type.TypeSystem.BuiltIn.I4 : destinationBase.Type.TypeSystem.BuiltIn.I8);

			context.SetInstruction(IRInstruction.UnstableObjectTracking);

			context.AppendInstruction(addInstruction, srcReg, sourceBase, source);
			context.AppendInstruction(addInstruction, dstReg, destinationBase, destination);

			var tmp = AllocateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I4);
			var tmpLarge = Is32BitPlatform && size >= 8 ? null : AllocateVirtualRegister(destinationBase.Type.TypeSystem.BuiltIn.I8);

			for (int i = 0; i < size;)
			{
				int left = size - i;

				var index = CreateConstant(i);

				if (left >= 8 & !Is32BitPlatform)
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
	}
}
