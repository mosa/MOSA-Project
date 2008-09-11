/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
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
			: base(type, register, -index) // HACK: Redo this with arch support!
		{
			_name = name;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the name of the variable if it is available.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		#endregion // Properties

		#region StackOperand overrides

        /// <summary>
        /// Clones the stack operand.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new LocalVariableOperand(this.Base, _name, -(base.Offset.ToInt32()/4), this.Type);
        }

        /// <summary>
        /// Returns a string representation of <see cref="Operand"/>.
        /// </summary>
        /// <returns>A string representation of the operand.</returns>
		public override string ToString()
        {
            return String.Format("{0} {1}", _name, base.ToString());
        }

        #endregion // StackOperand overrides
    }
}
