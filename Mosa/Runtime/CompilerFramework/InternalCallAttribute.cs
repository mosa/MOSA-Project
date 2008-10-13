/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// Marks a static method as a replaceable internal call.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InternalCallAttribute : Attribute
    {
        #region Data members

        /// <summary>
        /// Holds the call target provided by this function.
        /// </summary>
        private InternalCallTarget callTarget;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalCallAttribute"/> class.
        /// </summary>
        /// <param name="callTarget">The call target.</param>
        public InternalCallAttribute(InternalCallTarget callTarget)
        {
            this.callTarget = callTarget;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the call target.
        /// </summary>
        /// <value>The call target.</value>
        public InternalCallTarget CallTarget
        {
            get { return this.callTarget; }
        }

        #endregion // Properties
    }
}
