// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework
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
		public override string FamilyName { get { return "IR"; } }

		public override bool IsIRInstruction { get { return true; } }

		#endregion Properties
	}
}
