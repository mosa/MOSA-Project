// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.Transformation
{
	public class TransformContext
	{
		public MethodCompiler MethodCompiler { get; private set; }

		public TypeSystem TypeSystem { get { return MethodCompiler.TypeSystem; } }

		public TraceLog TraceLog { get; private set; }

		public TransformContext(MethodCompiler methodCompiler, TraceLog traceLog = null)
		{
			MethodCompiler = methodCompiler;
			TraceLog = traceLog;
		}

		public Operand AllocateVirtualRegister(MosaType type)
		{
			return MethodCompiler.VirtualRegisters.Allocate(type);
		}

		public bool ApplyTransform(Context context, BaseTransformation transformation, Stack<InstructionNode> worklist = null)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation.Name);

			if (worklist != null)
			{
				AddToWorkList(context, worklist);
			}

			transformation.Transform(context, this);

			TraceAfter(context);

			return true;
		}

		#region WorkList

		private void AddToWorkList(InstructionNode node, Stack<InstructionNode> worklist)
		{
			if (node.IsEmptyOrNop)
				return;

			// work list stays small, so the check is inexpensive
			if (worklist.Contains(node))
				return;

			worklist.Push(node);
		}

		private void AddToWorkList(Operand operand, Stack<InstructionNode> worklist)
		{
			if (!operand.IsVirtualRegister)
				return;

			foreach (var index in operand.Uses)
			{
				AddToWorkList(index, worklist);
			}

			foreach (var index in operand.Definitions)
			{
				AddToWorkList(index, worklist);
			}
		}

		private void AddToWorkList(Context context, Stack<InstructionNode> worklist)
		{
			if (context.Result != null)
			{
				AddToWorkList(context.Result, worklist);
			}
			if (context.Result2 != null)
			{
				AddToWorkList(context.Result2, worklist);
			}
			foreach (var operand in context.Operands)
			{
				AddToWorkList(operand, worklist);
			}
		}

		#endregion WorkList

		#region Trace

		public void TraceBefore(Context context, string name)
		{
			if (name != null)
				TraceLog?.Log($"*** {name}");

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
			return Operand.CreateConstant(TypeSystem.BuiltIn.U1, value);
		}

		protected Operand CreateConstant(int value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I4, value);
		}

		protected Operand CreateConstant(uint value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U4, value);
		}

		protected Operand CreateConstant(long value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I8, value);
		}

		protected Operand CreateConstant(ulong value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U8, value);
		}

		protected static Operand CreateConstant(MosaType type, long value)
		{
			return Operand.CreateConstant(type, (ulong)value);
		}

		protected static Operand CreateConstant(MosaType type, ulong value)
		{
			return Operand.CreateConstant(type, value);
		}

		protected static Operand CreateConstant(MosaType type, int value)
		{
			return Operand.CreateConstant(type, (long)value);
		}

		protected static Operand CreateConstant(MosaType type, uint value)
		{
			return Operand.CreateConstant(type, value);
		}

		protected Operand CreateConstant(float value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		protected Operand CreateConstant(double value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		#endregion Constant Helper Methods

		#region 64-Bit Helpers

		public void SetGetLow64(Context context, Operand operand1, Operand operand2)
		{
			if (operand2.IsResolvedConstant)
			{
				context.SetInstruction(IRInstruction.MoveInt32, operand1, CreateConstant(operand2.ConstantUnsignedInteger));
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
				context.SetInstruction(IRInstruction.MoveInt32, operand1, CreateConstant(operand2.ConstantUnsignedLongInteger >> 32));
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
				context.AppendInstruction(IRInstruction.MoveInt32, operand1, CreateConstant(operand2.ConstantUnsignedInteger));
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
				context.AppendInstruction(IRInstruction.MoveInt32, operand1, CreateConstant(operand2.ConstantUnsignedLongInteger >> 32));
			}
			else
			{
				context.AppendInstruction(IRInstruction.GetHigh64, operand1, operand2);
			}
		}

		#endregion 64-Bit Helpers
	}
}
