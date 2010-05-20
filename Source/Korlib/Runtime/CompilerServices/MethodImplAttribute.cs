/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited=false)]
    [Serializable]
    public sealed class MethodImplAttribute : Attribute
    {
        private MethodImplOptions options;

        public MethodImplAttribute()
        {
        }

        public MethodImplAttribute(short options)
        {
            this.options = (MethodImplOptions)options;
        }

        public MethodImplAttribute(MethodImplOptions options)
        {
            this.options = options;
        }

        public MethodImplOptions Value
        {
            get
            {
                return this.options;
            }
        }
    }
}
