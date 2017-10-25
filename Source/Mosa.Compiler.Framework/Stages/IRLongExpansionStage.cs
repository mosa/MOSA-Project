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
			AddVisitation(IRInstruction.LogicalAnd, LogicalAnd);
			AddVisitation(IRInstruction.LogicalOr, LogicalOr);
			AddVisitation(IRInstruction.LogicalXor, LogicalXor);
			AddVisitation(IRInstruction.LogicalNot, LogicalNot);
			AddVisitation(IRInstruction.LoadParameterInteger, LoadParameterInteger);
			AddVisitation(IRInstruction.LoadInteger, LoadInteger);
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
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;

			var resultLow = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var resultHigh = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			var context = new Context(node);

			if (operand2.IsConstant && !operand2.IsLong && !operand1.IsLong)
			{
				var target4 = CreateConstant((uint)(operand2.ConstantUnsignedLongInteger + 4));

				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, operand1, operand2);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, operand1, target4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			if (operand2.IsConstant && !operand2.IsLong && operand1.IsLong)
			{
				var target4 = CreateConstant((uint)(operand2.ConstantUnsignedLongInteger + 4));

				var op0Low = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var op0High = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

				context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, operand1);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, op0Low, operand2);
				context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, op0Low, target4);
				context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
				return;
			}

			return;
			//else
			//{
			//	Operand target4 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			//	var contant4 = Operand.CreateConstant(TypeSystem.BuiltIn.I4, 4);

			//	context.SetInstruction2(IRInstruction.Split64, op0Low, op0High, operand2);
			//	context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultLow, operand1, op0Low);
			//	context.AppendInstruction(IRInstruction.AddSigned, InstructionSize.Size32, target4, op0Low, contant4);
			//	context.AppendInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, resultHigh, operand1, target4);
			//	context.AppendInstruction(IRInstruction.To64, result, resultLow, resultHigh);
			//}
		}
	}
}
