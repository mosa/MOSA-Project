// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Intermediate representation for various IL call operations.
	/// </summary>
	/// <remarks>
	/// Instances of this class are used to represent call, calli and callvirt
	/// instructions.
	/// </remarks>
	/// <seealso cref="Mosa.Compiler.Framework.CIL.InvokeInstruction" />
	public sealed class CallInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CallInstruction" /> class.
		/// </summary>
		/// <param name="opCode">The op code.</param>
		public CallInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeSupportFlags InvokeSupport
		{
			get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
		}

		#endregion Properties
	}
}
