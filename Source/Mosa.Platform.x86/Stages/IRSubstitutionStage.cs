// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// IR Substitution Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class IRSubstitutionStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.RemFloatR4, RemFloatR4);
			AddVisitation(IRInstruction.RemFloatR8, RemFloatR8);
			AddVisitation(IRInstruction.RemSigned64, RemSigned64); // smod64
			AddVisitation(IRInstruction.DivSigned64, DivSigned64); // sdiv64
			AddVisitation(IRInstruction.RemUnsigned64, RemUnsigned64); // umod64
			AddVisitation(IRInstruction.DivUnsigned64, DivUnsigned64); // udiv64
		}

		#region Visitation Methods

		private void RemFloatR4(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR4);
			Debug.Assert(node.Operand1.IsR4);

			ReplaceWithPlatformDivisionCall(node, "RemR4", node.Result, node.Operand1, node.Operand2);
		}

		private void RemFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			ReplaceWithPlatformDivisionCall(node, "RemR8", node.Result, node.Operand1, node.Operand2);
		}

		private void RemSigned64(InstructionNode node)
		{
			ReplaceWithDivisionCall(node, "smod64", node.Result, node.Operand1, node.Operand2);
		}

		private void DivSigned64(InstructionNode node)
		{
			ReplaceWithDivisionCall(node, "sdiv64", node.Result, node.Operand1, node.Operand2);
		}

		private void RemUnsigned64(InstructionNode node)
		{
			ReplaceWithDivisionCall(node, "umod64", node.Result, node.Operand1, node.Operand2);
		}

		private void DivUnsigned64(InstructionNode node)
		{
			ReplaceWithDivisionCall(node, "udiv64", node.Result, node.Operand1, node.Operand2);
		}

		#endregion Visitation Methods

		private void ReplaceWithDivisionCall(InstructionNode node, string methodName, Operand result, Operand operand1, Operand operand2)
		{
			var type = TypeSystem.GetTypeByName("Mosa.Runtime.Math", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.Math.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2);

			MethodScanner.MethodInvoked(method, Method);
		}

		private void ReplaceWithPlatformDivisionCall(InstructionNode node, string methodName, Operand result, Operand operand1, Operand operand2)
		{
			var type = TypeSystem.GetTypeByName("Mosa.Runtime.Math.x86", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.x86.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2);

			MethodScanner.MethodInvoked(method, Method);
		}
	}
}
