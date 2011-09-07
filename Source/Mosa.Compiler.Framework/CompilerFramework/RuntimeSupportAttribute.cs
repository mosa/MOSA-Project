/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework.CIL;

namespace Mosa.Compiler.Framework
{

	/// <summary>
	/// Identifies runtime support for an CIL or IR opcode.
	/// </summary>
	/// <remarks>
	/// This attribute is used to mark runtime special methods. The attribute identifies a method
	/// to the compiler, which implements a specific opcode. Some opcodes require special runtime
	/// support, such as newobj, newarr. The jit and aot compilers require attributed methods to 
	/// successfully generate native code.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public sealed class RuntimeSupportAttribute : Attribute
	{
		#region Data members

		/// <summary>
		/// The opcode, which the attributed function supports.
		/// </summary>
		private OpCode _opCode;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeSupportAttribute"/> attribute.
		/// </summary>
		/// <param name="opcode">The opcode, which the attributed method supports.</param>
		public RuntimeSupportAttribute(OpCode opcode)
		{
			_opCode = opcode;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the opcode, which the attributed method supports.
		/// </summary>
		public OpCode OpCode
		{
			get { return _opCode; }
		}

		#endregion // Properties
	}
}
