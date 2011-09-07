/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Operands
{

	/// <summary>
	/// Represents a variable in source code, which also acts as an operand to IL instructions.
	/// </summary>
	/// <remarks>
	/// Variables are specific operands, which are backed by memory on the stack. The stack index
	/// of the variable is managed by this class. Negative stack indices represent method parameters 
	/// and are only supported for those.
	/// </remarks>
	public class LocalVariableOperand : StackOperand
	{
		#region Data members

		/// <summary>
		/// The name of the variable.
		/// </summary>
		private string _name;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes an instance of <see cref="LocalVariableOperand"/>.
		/// </summary>
		/// <param name="register">Holds the stack frame register.</param>
		/// <param name="name">The name of the variable.</param>
		/// <param name="index">Holds the variable index.</param>
		/// <param name="type">The type of the variable.</param>
		public LocalVariableOperand(Register register, string name, int index, SigType type)
			: base(type, register, -index) // HACK: Redo this with Architecture support!
		{
			_name = name;
		}

		#endregion // Construction

		#region StackOperand overrides

		/// <summary>
		/// Returns the name of the variable if it is available.
		/// </summary>
		public override string Name
		{
			get { return _name; }
		}

		#endregion // StackOperand overrides
	}
}
