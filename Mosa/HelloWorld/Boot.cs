
namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{
        static Boot()
        {
        }

		/// <summary>
		/// 
		/// </summary>
		public static void Main()
		{
            unsafe
            {
                byte* index = (byte*)(0xB8000);

                for (byte* i = (byte*)0xB8000; i < (byte*)0xB8FA0; )
                {
                    WriteChar((byte)0x00, i++);
                    WriteChar((byte)' ', i++);
                }

                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'M', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'O', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'S', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'A', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)' ', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'O', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'S', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)' ', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'V', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'e', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'r', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'s', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'i', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'o', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'n', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)' ', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'0', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'.', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'1', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)' ', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'\'', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'W', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'a', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'k', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'e', index++);
                WriteChar((byte)0x0A, index++);
                WriteChar((byte)'\'', index++);

                while (true)
                {
                    WriteChar((byte)0x0B, index++);
                    WriteChar((byte)'-', index--);
                    WriteChar((byte)0x0B, index++);
                    WriteChar((byte)'\\', index--);
                    WriteChar((byte)0x0B, index++);
                    WriteChar((byte)'|', index--);
                    WriteChar((byte)0x0B, index++);
                    WriteChar((byte)'/', index--);
                }
            }
        }

        /// <summary>
        /// Writes the char.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="address">The address.</param>
        public unsafe static void WriteChar(byte c, byte* address)
        {
            *address = c;
        }
    }
}
