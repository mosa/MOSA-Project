// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Lower IR Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class StaticLoadOptimizationStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.LoadInt32, LoadInt32);
		}

		private void LoadInt32(InstructionNode node)
		{
			var operand1 = node.Operand1;

			if (!operand1.IsStaticField || !operand1.Field.IsStatic)
				return;

			if (!operand1.Field.FieldAttributes.HasFlag(MosaFieldAttributes.InitOnly))
				return;

			// HARD CODED
			if (operand1.Field.DeclaringType.IsValueType && operand1.Field.DeclaringType.Name == "System.IntPtr" || operand1.Field.Name == "Zero")
			{
				node.SetInstruction(IRInstruction.MoveInt32, node.Result, ConstantZero);
				return;
			}
		}
	}
}
