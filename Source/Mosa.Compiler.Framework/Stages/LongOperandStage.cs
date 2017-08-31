// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Long Operand Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class LongOperandStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.LogicalAnd, LogicalAnd);
			AddVisitation(IRInstruction.LogicalOr, LogicalOr);
			AddVisitation(IRInstruction.LogicalXor, LogicalXor);
			AddVisitation(IRInstruction.LogicalNot, LogicalNot);
		}

		private void LogicalAnd(InstructionNode node)
		{
			if (!node.Result.Is64BitInteger)
				return;

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
			context.AppendInstruction(IRInstruction.LogicalAnd, InstructionSize.Size32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalAnd, InstructionSize.Size32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalOr(InstructionNode node)
		{
			if (!node.Result.Is64BitInteger)
				return;

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
			context.AppendInstruction(IRInstruction.LogicalOr, InstructionSize.Size32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalOr, InstructionSize.Size32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalXor(InstructionNode node)
		{
			if (!node.Result.Is64BitInteger)
				return;

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
			context.AppendInstruction(IRInstruction.LogicalXor, InstructionSize.Size32, resultLow, op0Low, op1Low);
			context.AppendInstruction(IRInstruction.LogicalXor, InstructionSize.Size32, resultHigh, op0High, op1High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}

		private void LogicalNot(InstructionNode node)
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
			context.AppendInstruction(IRInstruction.LogicalNot, InstructionSize.Size32, resultLow, op0Low);
			context.AppendInstruction(IRInstruction.LogicalNot, InstructionSize.Size32, resultHigh, op0High);
			context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
		}
	}
}
