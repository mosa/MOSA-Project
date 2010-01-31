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
    /// Used to mark specific language elements as architecture specific code.
    /// </summary>
    /// <remarks>
    /// Adding this attribute will exclude the language element From compilation, 
    /// if it the compilation architecture does not match the named architecture type.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Method|AttributeTargets.Property|AttributeTargets.Field|AttributeTargets.Enum|AttributeTargets.Delegate|AttributeTargets.Constructor|AttributeTargets.Assembly|AttributeTargets.Event|AttributeTargets.Interface|AttributeTargets.Module)]
    public sealed class ArchitectureSpecificAttribute : Attribute
    {
        #region Data members

        /// <summary>
        /// The architecture name string.
        /// </summary>
        private string _architecture;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ArchitectureSpecificAttribute"/>.
        /// </summary>
        /// <param name="architecture">The name of the architecture.</param>
        public ArchitectureSpecificAttribute(string architecture)
        {
            if (null == architecture)
                throw new ArgumentNullException(@"architecture");
            if (0 == architecture.Length)
                throw new ArgumentException(@"Must supply an architecture name.", @"architecture");

            _architecture = architecture;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the name of the architecture.
        /// </summary>
        public string Architecture
        {
            get { return _architecture; }
            set
            {
                if (null == value)
                    throw new ArgumentNullException(@"value");
                if (0 == value.Length)
                    throw new ArgumentException(@"Must supply an architecture name.", @"value");
                if (null != _architecture)
                    throw new InvalidOperationException(@"Can't change the architecture dynamically.");

                _architecture = value;
            }
        }

        #endregion // Properties
    }
}
