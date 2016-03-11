// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Represents a basic jump instruction.
	/// </summary>
	/// <remarks>
	/// Other more complex method invocation instructions derive from this class, specifically the CallInstruction,
	/// the CalliInstruction and CallvirtInstruction classes. They share the features provided by the JumpInstruction.
	/// </remarks>
	public sealed class JumpInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="JumpInstruction"/> class.
		/// </summary>
		public JumpInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
		{
			get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef; }
		}

		#endregion Properties
	}
}
