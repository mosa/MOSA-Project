// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// An abstract intermediate representation of the end of a finally block.
	/// </summary>
	public sealed class FinallyEnd : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="FinallyEnd"/>.
		/// </summary>
		public FinallyEnd() :
			base(0, 0)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets a value indicating whether to [ignore during code generation].
		/// </summary>
		/// <value>
		/// <c>true</c> if [ignore during code generation]; otherwise, <c>false</c>.
		/// </value>
		public override bool IgnoreDuringCodeGeneration { get { return true; } }

		#endregion Properties
	}
}
