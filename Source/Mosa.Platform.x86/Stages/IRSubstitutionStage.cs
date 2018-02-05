// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Platform Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class IRSubstitutionStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.RemFloatR4, RemFloatR4);
			AddVisitation(IRInstruction.RemFloatR8, RemFloatR8);
			AddVisitation(IRInstruction.RemSigned, RemSigned); // smod64
			AddVisitation(IRInstruction.DivSigned, DivSigned); // sdiv64
			AddVisitation(IRInstruction.RemUnsigned, RemUnsigned); // umod64
			AddVisitation(IRInstruction.DivUnsigned, DivUnsigned); // udiv64
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RemFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			ReplaceWithDivisionCall(node, "RemR4", node.Result, node.Operand1, node.Operand2);
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RemFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			ReplaceWithDivisionCall(node, "RemR8", node.Result, node.Operand1, node.Operand2);
		}

		/// <summary>
		/// Replaces the with division call.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="methodName">Name of the method.</param>
		/// <param name="result">The result.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		private void ReplaceWithDivisionCall(InstructionNode node, string methodName, Operand result, Operand operand1, Operand operand2)
		{
			var type = TypeSystem.GetTypeByName("Mosa.Runtime.x86", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.x86.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2);
		}

		/// <summary>
		/// Rems the signed.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RemSigned(InstructionNode node)
		{
			if (LongOperandStage.Any64Bit(node))
			{
				ReplaceWithDivisionCall(node, "smod64", node.Result, node.Operand1, node.Operand2);
			}
		}

		/// <summary>
		/// Divs the signed.
		/// </summary>
		/// <param name="node">The node.</param>
		private void DivSigned(InstructionNode node)
		{
			if (LongOperandStage.Any64Bit(node))
			{
				ReplaceWithDivisionCall(node, "sdiv64", node.Result, node.Operand1, node.Operand2);
			}
		}

		/// <summary>
		/// Rems the unsigned.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RemUnsigned(InstructionNode node)
		{
			if (LongOperandStage.Any64Bit(node))
			{
				ReplaceWithDivisionCall(node, "umod64", node.Result, node.Operand1, node.Operand2);
			}
		}

		/// <summary>
		/// Divs the unsigned.
		/// </summary>
		/// <param name="node">The node.</param>
		private void DivUnsigned(InstructionNode node)
		{
			if (LongOperandStage.Any64Bit(node))
			{
				ReplaceWithDivisionCall(node, "udiv64", node.Result, node.Operand1, node.Operand2);
			}
		}

		#endregion Visitation Methods
	}
}
