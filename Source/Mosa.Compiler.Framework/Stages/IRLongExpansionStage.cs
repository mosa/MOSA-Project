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
			AddVisitation(IRInstruction.LogicalAnd64, LogicalAnd64);
			AddVisitation(IRInstruction.LogicalOr64, LogicalOr64);
			AddVisitation(IRInstruction.LogicalXor64, LogicalXor64);
			AddVisitation(IRInstruction.LogicalNot64, LogicalNot64);
			AddVisitation(IRInstruction.LoadParameterInteger, LoadParameterInteger);
			AddVisitation(IRInstruction.LoadInteger, LoadInteger);

			AddVisitation(IRInstruction.MoveZeroExtended, MoveZeroExtended);
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
			context.AppendInstruction(IRInstruction.LogicalAnd32, InstructionSize.Size32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalAnd32, InstructionSize.Size32, resultHigh, op0High, op1High);
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
			context.AppendInstruction(IRInstruction.LogicalOr32, InstructionSize.Size32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalOr32, InstructionSize.Size32, resultHigh, op0High, op1High);
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
			context.AppendInstruction(IRInstruction.LogicalXor32, InstructionSize.Size32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalXor32, InstructionSize.Size32, resultHigh, op0High, op1High);
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

		private void LoadParameterInteger(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			// TODO: Managed 64bit pointers

			if (!node.Result.Is64BitInteger)
				return;

			var result = node.Result;
			var operand1 = node.Operand1;

			MethodCompiler.SplitLongOperand(operand1, out Operand op0Low, out Operand op0High);

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			context.SetInstruction(IRInstruction.LoadParameterInteger, InstructionSize.Size32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LoadParameterInteger, InstructionSize.Size32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LoadInteger(InstructionNode node)
		{
			Debug.Assert(!node.Result.IsR4);
			Debug.Assert(!node.Result.IsR8);

			if (!node.Result.Is64BitInteger)
				return;

			var result = node.Result;
			var location = node.Operand1;
			var offset = node.Operand2;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			if (offset.IsConstant && !offset.IsLong && !location.IsLong)
			{
				var offset4 = CreateConstant(offset.ConstantUnsignedLongInteger + 4u);

				context.SetInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, location, offset);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsConstant && !offset.IsLong && location.IsLong)
			{
				var offset4 = CreateConstant(offset.ConstantUnsignedLongInteger + 4u);

				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, location);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, op0Low, offset);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, op0Low, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (offset.IsVirtualRegister && !offset.IsLong && !location.IsLong)
			{
				var offset4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, location, offset);
				context.AppendInstruction(IRInstruction.AddUnsigned32, offset4, offset, CreateConstant(4u));
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, location, offset4);
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
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, op0Low, offset);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, op0Low, offset4);
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
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, location, op0Low);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, location, offset4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			return;
		}

		private void MoveZeroExtended(InstructionNode node)
		{
			if (!node.Result.Is64BitInteger)
				return;

			if (node.Result.Is64BitInteger)
				return;

			var context = new Context(node);

			context.SetInstruction(IRInstruction.To64, node.Result, node.Operand1, ConstantZero);
		}
	}
}
