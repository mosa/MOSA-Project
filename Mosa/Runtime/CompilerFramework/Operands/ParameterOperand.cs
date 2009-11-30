/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */


using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.Operands
{
    /// <summary>
    /// An operand, which represents a parameter of a method.
    /// </summary>
    public sealed class ParameterOperand : StackOperand
    {
        #region Data members

        /// <summary>
        /// The parameter object of the operand.
        /// </summary>
        private RuntimeParameter _parameter;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ParameterOperand"/>.
        /// </summary>
        /// <param name="register">The stack frame register.</param>
        /// <param name="param">The runtime parameter object, that represents this parameter.</param>
        /// <param name="type">The parameter type.</param>
        public ParameterOperand(Register register, RuntimeParameter param, SigType type)
            : base(type, register, param.Position)
        {
            _parameter = param;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Returns the runtime parameter structure of this operand.
        /// </summary>
        public RuntimeParameter Parameter
        {
            get { return _parameter; }
        }

        #endregion // Properties

        #region StackOperand overrides

        /// <summary>
        /// Retrieves the name of the stack operand.
        /// </summary>
        /// <value>The name of the stack operand.</value>
        public override string Name
        {
            get { return _parameter.Name; }
        }

        /// <summary>
        /// Clones the stack operand.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new ParameterOperand(Base, _parameter, _type);
        }
        
        #endregion // StackOperand overrides
    }
}


