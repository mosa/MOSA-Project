namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of alive hint.
	/// </summary>
	public sealed class Kill : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Kill"/> class.
		/// </summary>
		public Kill()
			: base(1, 0)
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