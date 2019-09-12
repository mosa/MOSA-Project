// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Transform
{
	public sealed class TransformContext
	{
		public MethodCompiler MethodCompiler { get; private set; }

		public TypeSystem TypeSystem { get { return MethodCompiler.TypeSystem; } }

		public TraceLog TraceLog { get; private set; }

		public Operand ConstantZero32 { get { return MethodCompiler.ConstantZero32; } }
		public Operand ConstantZero64 { get { return MethodCompiler.ConstantZero64; } }
		public Operand ConstantZeroR4 { get { return MethodCompiler.ConstantZeroR4; } }
		public Operand ConstantZeroR8 { get { return MethodCompiler.ConstantZeroR8; } }

		public MosaType I4 { get; private set; }
		public MosaType I8 { get; private set; }
		public MosaType R4 { get; private set; }
		public MosaType R8 { get; private set; }
		public MosaType O { get; private set; }

		public VirtualRegisters VirtualRegisters { get; private set; }

		public TransformContext(MethodCompiler methodCompiler, TraceLog traceLog = null)
		{
			MethodCompiler = methodCompiler;
			TraceLog = traceLog;

			VirtualRegisters = MethodCompiler.VirtualRegisters;

			I4 = TypeSystem.BuiltIn.I4;
			I8 = TypeSystem.BuiltIn.I8;
			R4 = TypeSystem.BuiltIn.R4;
			R8 = TypeSystem.BuiltIn.R8;
			O = TypeSystem.BuiltIn.Object;
		}

		public Operand AllocateVirtualRegister(MosaType type)
		{
			return VirtualRegisters.Allocate(type);
		}

		public bool ApplyTransform(Context context, BaseTransformation transformation, List<Operand> virtualRegisters = null)
		{
			if (!transformation.Match(context, this))
				return false;

			TraceBefore(context, transformation.Name);

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
	}
}
