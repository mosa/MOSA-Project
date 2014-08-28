namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of alive hint.
	/// </summary>
	public sealed class Gen : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Gen"/> class.
		/// </summary>
		public Gen()
			: base(0, 1)
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