/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// Implementation of the "System.String" class
	/// </summary>
    public class String
    {
        /*
         * Michael "grover" Froehlich, 2010/05/17:
         * 
         * Strings are unfortunately very special in their behavior and memory layout. The AOT compiler
         * expects strings to have a very specific layout in memory to be able to compile instructions,
         * such as ldstr ahead of time. This is only true as long as we don't have a real loader, app domains
         * and actual string interning, which would be used in the AOT too. Until that time has come,
         * the memory layout of String must match:
         * 
         * Object:
         *      - Methot Table Ptr
         *      - Sync Block
         * String:
         *      - Length
         *      - Variable length UTF-16 character buffer (should we switch to UTF-8 instead?)
         * 
         * This layout is used to generated strings in the AOT'd image in CILTransformationStage, which transforms
         * CIL instructions to IR and replaces ldstr by a load from the data section, which contains the string laid
         * out along the above lines.
         * 
         */

		/// <summary>
		/// Length
		/// </summary>
		private int length;

        public int Length
        {
            get
            {
                return this.length;
            }
        }
    }
}
