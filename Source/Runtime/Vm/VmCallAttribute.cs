/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// Used to mark a method with the <see cref="VmCall"/> to invoke.
    /// </summary>
    public sealed class VmCallAttribute : InternalCallImplAttribute
    {
        #region Data members

        /// <summary>
        /// Holds the runtime call represented by this attribute.
        /// </summary>
        private VmCall runtimeCall;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="VmCallAttribute"/> class.
        /// </summary>
        /// <param name="runtimeCall">The runtime call.</param>
        public VmCallAttribute(VmCall runtimeCall)
        {
            this.runtimeCall = runtimeCall;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the runtime call represented by this attribute.
        /// </summary>
        /// <value>The runtime call of this attribute.</value>
        public VmCall RuntimeCall
        {
            get { return this.runtimeCall; }
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
        public sealed override bool Match(InternalCallImplAttribute call)
        {
            VmCallAttribute vmCall = call as VmCallAttribute;
            return (vmCall != null && vmCall.RuntimeCall == this.RuntimeCall);
        }

        #endregion // InternalCallImplAttribute Overrides
    }
}
