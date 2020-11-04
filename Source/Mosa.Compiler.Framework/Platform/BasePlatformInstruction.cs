// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// Base Platform Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseInstruction" />
	public abstract class BasePlatformInstruction : BaseInstruction
	{
		#region Properties

		public override bool IsPlatformInstruction { get { return true; } }

		#endregion Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasePlatformInstruction"/> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected BasePlatformInstruction(byte resultCount, byte operandCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction
	}
}
