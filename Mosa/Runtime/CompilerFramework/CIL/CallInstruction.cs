/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// Intermediate representation for various IL call operations.
	/// </summary>
	/// <remarks>
	/// Instances of this class are used to represent call, calli and callvirt
	/// instructions.
	/// </remarks>
	public class CallInstruction : InvokeInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CallInstruction"/> class.
		/// </summary>
		public CallInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the supported immediate metadata tokens in the instruction.
		/// </summary>
		/// <value></value>
		protected override InvokeInstruction.InvokeSupportFlags InvokeSupport
		{
			get { return InvokeSupportFlags.MemberRef | InvokeSupportFlags.MethodDef | InvokeSupportFlags.MethodSpec; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(CILVisitor vistor, Context context)
		{
			vistor.Call(context);
		}

		#endregion // Methods

	}
}
