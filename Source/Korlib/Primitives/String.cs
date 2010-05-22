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
    using System.Runtime.CompilerServices;

    using Mosa.Runtime.CompilerFramework;

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

        [IndexerName("Chars")]
        public unsafe char this[int index]
        {
            get
            {
                char result;

                fixed (int *pLength = &this.length)
                {
                    char* pChars = (char*)(pLength + 1);
                    result = *(pChars + index);
                }

                return result;
            }
        }

        // FIXME: These should be char,int instead of int,int; but that doesn't compile in MOSA for type matching reasons
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern String(char c, int count);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern String(char[] value);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern String(char[] value, int startIndex, int length);

        ////[MethodImplAttribute(MethodImplOptions.InternalCall)]
        ////public unsafe extern String(sbyte* value);

        ////[MethodImplAttribute(MethodImplOptions.InternalCall)]
        ////public unsafe extern String(sbyte* value, int startIndex, int length);

        ////[MethodImplAttribute(MethodImplOptions.InternalCall)]
        ////public unsafe extern String(sbyte* value, int startIndex, int length, Encoding enc);

        ////[MethodImplAttribute(MethodImplOptions.InternalCall)]
        ////public unsafe extern String(char* value);

        ////[MethodImplAttribute(MethodImplOptions.InternalCall)]
        ////public unsafe extern String(char* value, int startIndex, int length);

        private static unsafe string CreateString(int c, int count)
        {
            String result = InternalAllocateString(count);
            char ch = (char)c;

            fixed (int* pLength = &result.length)
            {
                char* pChars = (char*)(pLength + 1);
                
                while (count > 0)
                {
                    *pChars = ch;
                    count--;
                    pChars++;
                }
            }

            return result;
        }

        private static string CreateString(char[] value)
        {
            return CreateString(value, 0, value.Length);
        }

        private static unsafe string CreateString(char[] value, int startIndex, int length)
        {
            String result = InternalAllocateString(length);

            fixed (int* pLength = &result.length)
            {
                char* pChars = (char*)(pLength + 1);

                for (int index = startIndex; index < startIndex + length; index++)
                {
                    *pChars = value[index];
                    pChars++;
                }
            }

            return result;
        }

        [Intrinsic(@"Mosa.Runtime.CompilerFramework.Intrinsics.InternalAllocateString, Mosa.Runtime")]
        private static string InternalAllocateString(int length)
        {
            //throw new NotSupportedException(@"Can't run this code outside of MOSA.");
            return null;
        }
    }
}
