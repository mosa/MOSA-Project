
namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Intermediate representation of call context.
	/// </summary>
	public sealed class IntrinsicMethodCall : BaseIRInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Call"/> class.
		/// </summary>
		public IntrinsicMethodCall()
		{
		}

		#endregion // Construction

		#region IRInstruction Overrides

		/// <summary>
		/// Abstract visitor method for intermediate representation visitors.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IIRVisitor visitor, Context context)
		{
			visitor.IntrinsicMethodCall(context);
		}

		#endregion // IRInstruction Overrides
	}
}
