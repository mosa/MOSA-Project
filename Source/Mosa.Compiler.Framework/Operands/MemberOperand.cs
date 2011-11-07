/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// An operand, which represents a runtime member to be resolved at link time.
	/// </summary>
	public sealed class MemberOperand : MemoryOperand
	{
		#region Data members

		/// <summary>
		/// Holds the members.
		/// </summary>
		private readonly RuntimeMember member;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MemberOperand"/>.
		/// </summary>
		/// <param name="field">The runtime field to reference.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="field"/> is null.</exception>
		public MemberOperand(RuntimeField field) :
			base(field.SignatureType, null, IntPtr.Zero)
		{
			if (field == null)
				throw new ArgumentNullException(@"field");

			this.member = field;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberOperand"/> class.
		/// </summary>
		/// <param name="method">The method to reference.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="method"/> is null.</exception>
		public MemberOperand(RuntimeMethod method) :
			base(BuiltInSigType.IntPtr, null, IntPtr.Zero)
		{
			if (method == null)
				throw new ArgumentNullException(@"method");

			this.member = method;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberOperand"/> class.
		/// </summary>
		/// <param name="member">The member to reference.</param>
		/// <param name="type">The type of data held in the operand.</param>
		/// <param name="offset">The offset from the base register or absolute address to retrieve.</param>
		public MemberOperand(RuntimeMember member, SigType type, IntPtr offset) :
			base(type, null, offset)
		{
			if (member == null)
				throw new ArgumentNullException(@"member");

			this.member = member;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the runtime member of this operand.
		/// </summary>
		/// <value>The runtime member to link against.</value>
		public RuntimeMember Member
		{
			get { return this.member; }
		}

		#endregion // Properties

		#region Object Overrides

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			return String.Format("{0} [{1}]", this.member, this.Type);
		}

		#endregion // Object Overrides
	}
}
