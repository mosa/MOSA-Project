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
	}
}