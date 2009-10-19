/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// This class is the real System.Object.
    /// </summary>
    /// <remarks>
    /// This class defines the runtime layout of the first couple of bytes of any object. Mostly contains 
    /// the type information via method table, garbage collector info and synchronization data.
    /// </remarks>
    public abstract class Object
    {
        /// <summary>
        /// 
        /// </summary>
        private MethodTable _methodTable;

        /// <summary>
        /// Gets or sets the method table.
        /// </summary>
        /// <value>The method table.</value>
        internal MethodTable MethodTable
        {
            get { return _methodTable; }
            set { _methodTable = value; }
        }
    }
}
