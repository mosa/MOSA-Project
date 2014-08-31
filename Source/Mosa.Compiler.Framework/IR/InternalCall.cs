namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of internal call context.
	/// </summary>
	public sealed class InternalCall : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InternalCall"/> class.
		/// </summary>
		public InternalCall()
			: base(0, 0)
		{
		}

		#endregion Construction

		#region IRInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.InternalCall(context);
		}

		#endregion IRInstruction Overrides
	}
}