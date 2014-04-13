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
	}
}