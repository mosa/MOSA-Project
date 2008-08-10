/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
        private MethodTable _methodTable;
    }
}
