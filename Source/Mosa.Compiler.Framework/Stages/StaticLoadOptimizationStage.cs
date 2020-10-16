// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;

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
			AddVisitation(IRInstruction.Load32, LoadInt32);
			AddVisitation(IRInstruction.Load64, LoadInt64);
		}

		private void LoadInt32(Context context)
		{
			var operand1 = context.Operand1;

			if (!operand1.IsStaticField || !operand1.Field.IsStatic)
				return;

			if ((operand1.Field.FieldAttributes & MosaFieldAttributes.InitOnly) == 0)
				return;

			// HARD CODED
			if (operand1.Field.DeclaringType.IsValueType && (operand1.Field.DeclaringType.Name == "System.IntPtr" || operand1.Field.DeclaringType.Name == "System.UIntPtr") && operand1.Field.Name == "Zero")
			{
				context.SetInstruction(IRInstruction.Move32, context.Result, ConstantZero);
				return;
			}
		}

		private void LoadInt64(Context context)
		{
			var operand1 = context.Operand1;

			if (!operand1.IsStaticField || !operand1.Field.IsStatic)
				return;

			if ((operand1.Field.FieldAttributes & MosaFieldAttributes.InitOnly) == 0)
				return;

			// HARD CODED
			if (operand1.Field.DeclaringType.IsValueType && (operand1.Field.DeclaringType.Name == "System.IntPtr" || operand1.Field.DeclaringType.Name == "System.UIntPtr") && operand1.Field.Name == "Zero")
			{
				context.SetInstruction(IRInstruction.Move64, context.Result, ConstantZero);
				return;
			}
		}
	}
}
