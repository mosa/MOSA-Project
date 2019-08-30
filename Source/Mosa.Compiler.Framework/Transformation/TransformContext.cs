// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transformation
{
	public sealed class TransformContext
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

		public bool ApplyTransform(Context context, BaseTransformation transformation, Stack<Operand> virtualRegisters = null)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation.Name);

			if (virtualRegisters != null)
			{
				CollectVirtualRegisters(context, virtualRegisters);
			}

			transformation.Transform(context, this);

			TraceAfter(context);

			return true;
		}

		#region WorkList

		private void CollectVirtualRegisters(Operand operand, Stack<Operand> virtualRegisters)
		{
			// work list stays small, so the check is inexpensive
			if (virtualRegisters.Contains(operand))
				return;

			virtualRegisters.Push(operand);
		}

		private void CollectVirtualRegisters(Context context, Stack<Operand> virtualRegisters)
		{
			if (context.Result != null)
			{
				CollectVirtualRegisters(context.Result, virtualRegisters);
			}
			if (context.Result2 != null)
			{
				CollectVirtualRegisters(context.Result2, virtualRegisters);
			}
			foreach (var operand in context.Operands)
			{
				CollectVirtualRegisters(operand, virtualRegisters);
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

		public Operand CreateConstant(int value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I4, value);
		}

		public Operand CreateConstant(uint value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U4, value);
		}

		public Operand CreateConstant(long value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.I8, value);
		}

		public Operand CreateConstant(ulong value)
		{
			return Operand.CreateConstant(TypeSystem.BuiltIn.U8, value);
		}

		public static Operand CreateConstant(MosaType type, long value)
		{
			return Operand.CreateConstant(type, (ulong)value);
		}

		public static Operand CreateConstant(MosaType type, ulong value)
		{
			return Operand.CreateConstant(type, value);
		}

		public static Operand CreateConstant(MosaType type, int value)
		{
			return Operand.CreateConstant(type, (long)value);
		}

		public static Operand CreateConstant(MosaType type, uint value)
		{
			return Operand.CreateConstant(type, value);
		}

		public Operand CreateConstant(float value)
		{
			return Operand.CreateConstant(value, TypeSystem);
		}

		public Operand CreateConstant(double value)
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
