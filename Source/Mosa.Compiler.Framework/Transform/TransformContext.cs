﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transform
{
	public sealed class TransformContext
	{
		public MethodCompiler MethodCompiler { get; private set; }
		public Compiler Compiler { get; private set; }

		public TypeSystem TypeSystem { get; private set; }

		public TraceLog TraceLog { get; private set; }

		public TraceLog SpecialTraceLog { get; private set; }

		public Operand ConstantZero32 { get; private set; }
		public Operand ConstantZero64 { get; private set; }
		public Operand ConstantZeroR4 { get; private set; }
		public Operand ConstantZeroR8 { get; private set; }

		public MosaType I4 { get; private set; }
		public MosaType I8 { get; private set; }
		public MosaType R4 { get; private set; }
		public MosaType R8 { get; private set; }
		public MosaType O { get; private set; }

		public VirtualRegisters VirtualRegisters { get; private set; }

		public bool LowerTo32 { get; private set; }

		public TransformContext(MethodCompiler methodCompiler)
		{
			MethodCompiler = methodCompiler;
			Compiler = methodCompiler.Compiler;

			TypeSystem = Compiler.TypeSystem;

			VirtualRegisters = MethodCompiler.VirtualRegisters;

			I4 = TypeSystem.BuiltIn.I4;
			I8 = TypeSystem.BuiltIn.I8;
			R4 = TypeSystem.BuiltIn.R4;
			R8 = TypeSystem.BuiltIn.R8;
			O = TypeSystem.BuiltIn.Object;

			ConstantZero32 = MethodCompiler.ConstantZero32;
			ConstantZero64 = MethodCompiler.ConstantZero64;
			ConstantZeroR4 = MethodCompiler.ConstantZeroR4;
			ConstantZeroR8 = MethodCompiler.ConstantZeroR8;

			LowerTo32 = Compiler.CompilerSettings.LongExpansion;
		}

		public void SetLogs(TraceLog traceLog = null, TraceLog specialTraceLog = null)
		{
			TraceLog = traceLog;
			SpecialTraceLog = specialTraceLog;
		}

		public void SetStageOptions(bool lowerTo32)
		{
			LowerTo32 = Compiler.CompilerSettings.LongExpansion && lowerTo32;
		}

		public Operand AllocateVirtualRegister(MosaType type)
		{
			return VirtualRegisters.Allocate(type);
		}

		public Operand AllocateVirtualRegister32()
		{
			return VirtualRegisters.Allocate(I4);
		}

		public Operand AllocateVirtualRegister64()
		{
			return VirtualRegisters.Allocate(I8);
		}

		public Operand AllocateVirtualRegisterR4()
		{
			return VirtualRegisters.Allocate(R4);
		}

		public Operand AllocateVirtualRegisterR8()
		{
			return VirtualRegisters.Allocate(R8);
		}

		public Operand AllocateVirtualRegisterObject()
		{
			return VirtualRegisters.Allocate(O);
		}

		public bool ApplyTransform(Context context, BaseTransformation transformation, List<Operand> virtualRegisters = null)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation);

			if (virtualRegisters != null)
			{
				CollectVirtualRegisters(context, virtualRegisters);
			}

			// TODO: note last virtual register #
			// TODO: note the node

			transformation.Transform(context, this);

			// TODO: add all NEW virtual register to the collection

			TraceAfter(context);

			return true;
		}

		#region WorkList

		private static void CollectVirtualRegisters(Context context, List<Operand> virtualRegisters)
		{
			if (context.Result != null)
			{
				virtualRegisters.AddIfNew(context.Result);
			}
			if (context.Result2 != null)
			{
				virtualRegisters.AddIfNew(context.Result2);
			}
			foreach (var operand in context.Operands)
			{
				virtualRegisters.AddIfNew(operand);
			}
		}

		#endregion WorkList

		#region Trace

		public void TraceBefore(Context context, BaseTransformation transformation)
		{
			if (transformation.Name != null)
				TraceLog?.Log($"*** {transformation.Name}");

			if (transformation.Log)
				SpecialTraceLog?.Log($"{transformation.Name}\t{MethodCompiler.Method.FullName} at {context}");

			TraceLog?.Log($"BEFORE:\t{context}");
		}

		public void TraceAfter(Context context)
		{
			TraceLog?.Log($"AFTER: \t{context}");
		}

		#endregion Trace

		#region Constant Helper Methods

		public Operand CreateConstant(byte value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(TypeSystem.BuiltIn.U1, value);
		}

		public Operand CreateConstant(int value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(TypeSystem.BuiltIn.I4, value);
		}

		public Operand CreateConstant(uint value)
		{
			return value == 0 ? ConstantZero32 : Operand.CreateConstant(TypeSystem.BuiltIn.U4, value);
		}

		public Operand CreateConstant(long value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(TypeSystem.BuiltIn.I8, value);
		}

		public Operand CreateConstant(ulong value)
		{
			return value == 0 ? ConstantZero64 : Operand.CreateConstant(TypeSystem.BuiltIn.U8, value);
		}

		public Operand CreateConstant(float value)
		{
			return value == 0 ? ConstantZeroR4 : Operand.CreateConstant(value, TypeSystem);
		}

		public Operand CreateConstant(double value)
		{
			return value == 0 ? ConstantZeroR4 : Operand.CreateConstant(value, TypeSystem);
		}

		#endregion Constant Helper Methods

		#region 64-Bit Helpers

		public void SetGetLow64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned32));
			}
			else
			{
				context.SetInstruction(IRInstruction.GetLow64, operand1, operand2);
			}
		}

		public void SetGetHigh64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned64 >> 32));
			}
			else
			{
				context.SetInstruction(IRInstruction.GetHigh64, operand1, operand2);
			}
		}

		public void AppendGetLow64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.AppendInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned32));
			}
			else
			{
				context.AppendInstruction(IRInstruction.GetLow64, operand1, operand2);
			}
		}

		public void AppendGetHigh64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.AppendInstruction(IRInstruction.Move32, operand1, CreateConstant(operand2.ConstantUnsigned64 >> 32));
			}
			else
			{
				context.AppendInstruction(IRInstruction.GetHigh64, operand1, operand2);
			}
		}

		#endregion 64-Bit Helpers

		public void UpdatePHI(Context context)
		{
			Debug.Assert(context.OperandCount != context.Block.PreviousBlocks.Count);

			// One of the previous blocks was removed, fix up the operand blocks

			var node = context.Node;
			var previousBlocks = node.Block.PreviousBlocks;
			var phiBlocks = node.PhiBlocks;

			for (int index = 0; index < node.OperandCount; index++)
			{
				var phiBlock = phiBlocks[index];

				if (previousBlocks.Contains(phiBlock))
					continue;

				phiBlocks.RemoveAt(index);

				for (int i = index; index < node.OperandCount - 1; index++)
				{
					context.SetOperand(i, node.GetOperand(i + 1));
				}

				context.SetOperand(node.OperandCount - 1, null);
				node.OperandCount--;
			}

			Debug.Assert(context.OperandCount == context.Block.PreviousBlocks.Count);
		}

		public void SplitLongOperand(Operand operand, out Operand operandLow, out Operand operandHigh)
		{
			MethodCompiler.SplitLongOperand(operand, out operandLow, out operandHigh);
		}
	}
}
