// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Abstract base class for all instructions in the intermediate representation.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseInstruction" />
	public abstract class BaseIRInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="BaseIRInstruction"/>.
		/// </summary>
		/// <param name="operandCount">Specifies the number of operands of the context.</param>
		/// <param name="resultCount">Specifies the number of results of the context.</param>
		protected BaseIRInstruction(byte operandCount, byte resultCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public override string InstructionFamilyName { get { return "IR"; } }

		/// <summary>
		/// Gets a value indicating whether [variable operand count].
		/// </summary>
		/// <value>
		///   <c>true</c> if [variable operand count]; otherwise, <c>false</c>.
		/// </value>
		public virtual bool VariableOperandCount { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="BaseIRInstruction"/> is commutative.
		/// </summary>
		/// <value>
		///   <c>true</c> if commutative; otherwise, <c>false</c>.
		/// </value>
		public virtual bool Commutative { get { return false; } }

		/// <summary>
		/// Gets a value indicating whether this instance has side effect.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance has side effect; otherwise, <c>false</c>.
		/// </value>
		public virtual bool HasSideEffect { get { return true; } }

		#endregion Properties
	}
}
