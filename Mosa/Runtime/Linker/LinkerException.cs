/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Runtime.Serialization;

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.Linker
{
    /// <summary>
    /// Indicates linker exceptions, such as unresolved symbols or duplicate symbols.
    /// </summary>
    [Serializable]
    public class LinkerException : CompilationException
    {
        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerException"/> class.
        /// </summary>
        public LinkerException()
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LinkerException(string message) : 
            base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public LinkerException(string message, Exception inner) : 
            base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerException"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected LinkerException(SerializationInfo info, StreamingContext context) : 
            base(info, context)
        {
        }

        #endregion // Construction
    }
}
