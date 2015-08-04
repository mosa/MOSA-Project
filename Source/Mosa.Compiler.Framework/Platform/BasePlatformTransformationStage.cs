// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Linker;

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	///
	/// </summary>
	public abstract class BasePlatformTransformationStage : BaseCodeTransformationStage
	{
		protected virtual string Platform { get { return "Generic"; } }

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public override string Name { get { return Platform + "." + base.Name; } }

		#endregion IPipelineStage Members

		#region Emit Constant Methods

		/// <summary>
		/// Emits the constant operands.
		/// </summary>
		/// <param name="node">The node.</param>
		protected void EmitFloatingPointConstants(InstructionNode node)
		{
			if (node.OperandCount > 0)
				node.Operand1 = EmitFloatingPointConstant(node.Operand1);
			if (node.OperandCount > 1)
				node.Operand2 = EmitFloatingPointConstant(node.Operand2);
			if (node.OperandCount > 2)
				node.Operand3 = EmitFloatingPointConstant(node.Operand3);
		}

		/// <summary>
		/// This function emits a constant variable into the read-only data section.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns>
		/// An operand, which represents the reference to the read-only constant.
		/// </returns>
		protected Operand EmitFloatingPointConstant(Operand operand)
		{
			if (!operand.IsConstant || !operand.IsR)
				return operand;

			LinkerSymbol symbol;

			if (operand.IsR4)
			{
				symbol = MethodCompiler.Linker.GetConstantSymbol(operand.ConstantSingleFloatingPoint);
			}
			else
			{
				symbol = MethodCompiler.Linker.GetConstantSymbol(operand.ConstantDoubleFloatingPoint);
			}

			return Operand.CreateLabel(operand.Type, symbol.Name);
		}

		#endregion Emit Constant Methods
	}
}
