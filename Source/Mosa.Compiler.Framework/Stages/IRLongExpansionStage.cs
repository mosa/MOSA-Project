// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Long Expansion Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class IRLongExpansionStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.LoadParameterInteger64, LoadParameterInteger64);
			AddVisitation(IRInstruction.LoadInteger64, LoadInteger64);
			AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			AddVisitation(IRInstruction.Truncation64x32, Truncation64x32);
			AddVisitation(IRInstruction.ZeroExtended32x64, ZeroExtended32x64);
		}

		#region Visitation Methods

		private void LoadInteger64(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			Debug.Assert(node.Result.Is64BitInteger);

			var result = node.Result;
			var location = node.Operand1;
			var offset = node.Operand2;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			if (offset.IsConstant && !offset.IsLong && !location.IsLong)
			{
				var offset4 = CreateConstant(offset.ConstantUnsignedLongInteger + 4u);

				context.SetInstruction(IRInstruction.LoadInteger32, resultLow, location, offset);
				context.AppendInstruction(IRInstruction.LoadInteger32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsConstant && !offset.IsLong && location.IsLong)
			{
				var offset4 = CreateConstant(offset.ConstantUnsignedLongInteger + 4u);

				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, location);
				context.AppendInstruction(IRInstruction.LoadInteger32, resultLow, op0Low, offset);
				context.AppendInstruction(IRInstruction.LoadInteger32, resultHigh, op0Low, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsVirtualRegister && !offset.IsLong && !location.IsLong)
			{
				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.LoadInteger32, resultLow, location, offset);
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, offset, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInteger32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsVirtualRegister && !offset.IsLong && location.IsLong)
			{
				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, location);
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, offset, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInteger32, resultLow, op0Low, offset);
				context.AppendInstruction(IRInstruction.LoadInteger32, resultHigh, op0Low, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsConstant && offset.IsLong && !location.IsLong)
			{
				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, offset);
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, op0Low, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInteger32, resultLow, location, op0Low);
				context.AppendInstruction(IRInstruction.LoadInteger32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			return;
		}

		private void LoadParameterInteger64(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);
			Debug.Assert(node.Result.Is64BitInteger);

			// TODO: Managed 64bit pointers

			var result = node.Result;
			var operand1 = node.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand op0High);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction(IRInstruction.LoadParameterInteger32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LoadParameterInteger32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalAnd64(InstructionNode node)
		{
			Debug.Assert(node.Result.Is64BitInteger);

			var result = node.Result;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, operand1);
			context.AppendInstruction2(IRInstruction.Split64, op1Low, op1High, operand2);
			context.AppendInstruction(IRInstruction.LogicalAnd32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalAnd32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalNot64(InstructionNode node)
		{
			if (!node.Result.Is64BitInteger)
				return;

			var result = node.Result;
			var operand1 = node.Operand1;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, operand1);
			context.AppendInstruction(IRInstruction.LogicalNot32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LogicalNot32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalOr64(InstructionNode node)
		{
			Debug.Assert(node.Result.Is64BitInteger);

			var result = node.Result;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, operand1);
			context.AppendInstruction2(IRInstruction.Split64, op1Low, op1High, operand2);
			context.AppendInstruction(IRInstruction.LogicalOr32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalOr32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalXor64(InstructionNode node)
		{
			Debug.Assert(node.Result.Is64BitInteger);

			var result = node.Result;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;

			var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var op1High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, operand1);
			context.AppendInstruction2(IRInstruction.Split64, op1Low, op1High, operand2);
			context.AppendInstruction(IRInstruction.LogicalXor32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalXor32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void Truncation64x32(InstructionNode node)
		{
			Debug.Assert(node.Operand1.Is64BitInteger);
			Debug.Assert(!node.Result.Is64BitInteger);

			var result = node.Result;
			var operand1 = node.Operand1;

			var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction2(IRInstruction.Split64, result, op0High, operand1);
		}

		private void ZeroExtended32x64(InstructionNode node)
		{
			node.SetInstruction(IRInstruction.To64, node.Result, node.Operand1, ConstantZero);
		}

		#endregion Visitation Methods
	}
}
