/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.Operands
{
    /// <summary>
    /// A temporary stack local operand used to capture results From CIL operations.
    /// </summary>
    /// <remarks>
    /// Temporaries have a stack slot assigned to them however these, see VariableOperand and
    /// ParameterOperand can be optimized by a register allocator and move to registers.
    /// </remarks>
    public sealed class TemporaryOperand : StackOperand
    {
        #region Data members

        /// <summary>
        /// Stores the label of the instruction, that created the temporary operand.
        /// </summary>
        private readonly int _label;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the virtual register.
        /// </summary>
        /// <param name="label">The virtual register number.</param>
        /// <param name="typeRef">The type reference of the virtual register.</param>
        /// <param name="register">The stack base register.</param>
        /// <param name="offset">The offset.</param>
        public TemporaryOperand(int label, SigType typeRef, Register register, int offset)
            : base(typeRef, register, offset)
        {
            _label = label;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the id of the virtual register.
        /// </summary>
        public int Id { get { return _label; } }

        #endregion // Properties

        #region StackOperand Overrides

        /// <summary>
        /// Retrieves the name of the stack operand.
        /// </summary>
        /// <value>The name of the stack operand.</value>
        public override string Name
        {
            get { return _label.ToString(); }
        }

        #endregion // StackOperand Overrides

        #region ICloneable Members

        /// <summary>
        /// Clones an operand.
        /// </summary>
        /// <returns>A new instance of TemporaryOperand.</returns>
        public override object Clone()
        {
            return new TemporaryOperand(_label, _type, Base, Offset.ToInt32());
        }

        #endregion // ICloneable Members        
    }
}


