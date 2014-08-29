namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of kill all.
	/// </summary>
	public sealed class KillAll : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="KillAll" /> class.
		/// </summary>
		public KillAll()
			: base(0, 0)
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