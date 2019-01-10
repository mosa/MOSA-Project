// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Platform;

namespace Mosa.Platform.ESP32
{
	/// <summary>
	/// ESP32 Instruction
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.BasePlatformInstruction" />
	public abstract class ESP32Instruction : BasePlatformInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ESP32Instruction" /> class.
		/// </summary>
		/// <param name="resultCount">The result count.</param>
		/// <param name="operandCount">The operand count.</param>
		protected ESP32Instruction(byte resultCount, byte operandCount)
			: base(resultCount, operandCount)
		{
		}

		#endregion Construction

		/// <summary>
		/// Gets the name of the instruction family.
		/// </summary>
		/// <value>
		/// The name of the instruction family.
		/// </value>
		public override string FamilyName { get { return "ESP32"; } }
	}
}
