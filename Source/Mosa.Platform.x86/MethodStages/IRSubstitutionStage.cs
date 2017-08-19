// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Platform.x86.MethodStages
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

			RemFloat(node, "RemR4");
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RemFloatR8(InstructionNode node)
		{
			Debug.Assert(node.Result.IsR8);
			Debug.Assert(node.Operand1.IsR8);

			RemFloat(node, "RemR8");
		}

		/// <summary>
		/// Visitation function for RemFloat.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="methodName">The method.</param>
		private void RemFloat(InstructionNode node, string methodName)
		{
			var result = node.Result;
			var dividend = node.Operand1;
			var divisor = node.Operand2;

			var type = TypeSystem.GetTypeByName("Mosa.Runtime.x86", "Division");

			Debug.Assert(type != null, "Cannot find type: Mosa.Runtime.x86.Division type");

			var method = type.FindMethodByName(methodName);

			Debug.Assert(methodName != null, "Cannot find method: " + methodName);

			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, dividend, divisor);
		}

		#endregion Visitation Methods
	}
}
