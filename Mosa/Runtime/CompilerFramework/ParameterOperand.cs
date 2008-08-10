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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework
{
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

        #region Object overrides

        public override object Clone()
        {
            return new ParameterOperand(base.Base, _parameter, _type);
        }

        public override string ToString()
        {
            return String.Format("{0} [{1}]", _parameter.Name, _type);
        }

        #endregion // Object overrides
    }
}
