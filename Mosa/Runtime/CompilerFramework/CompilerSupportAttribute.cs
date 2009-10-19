/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Marks a static method as a replaceable internal call.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CompilerSupportAttribute : Vm.InternalCallImplAttribute
    {
        #region Data members

        /// <summary>
        /// Holds the call target provided by this function.
        /// </summary>
        private CompilerSupportFunction supportFunction;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CompilerSupportAttribute"/> class.
        /// </summary>
        /// <param name="supportFunction">The call target.</param>
        public CompilerSupportAttribute(CompilerSupportFunction supportFunction)
        {
            this.supportFunction = supportFunction;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the call target.
        /// </summary>
        /// <value>The call target.</value>
        public CompilerSupportFunction SupportFunction
        {
            get { return supportFunction; }
        }

        #endregion // Properties

        #region InternalCallImplAttribute Overrides

        /// <summary>
        /// Checks if this attribute matches the specified attribute.
        /// </summary>
        /// <param name="call">The call attribute.</param>
        /// <returns>
        /// 	<c>true</c> if they match; otherwise <c>false</c>.
        /// </returns>
        public override bool Match(Vm.InternalCallImplAttribute call)
        {
            CompilerSupportAttribute csa = call as CompilerSupportAttribute;
            return (null != csa && csa.SupportFunction == supportFunction);
        }

        #endregion // InternalCallImplAttribute Overrides
    }
}
