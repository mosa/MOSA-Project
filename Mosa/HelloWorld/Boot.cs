
namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Initializes the <see cref="Boot"/> class.
		/// </summary>
		static Boot()
		{
		}

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			unsafe {

				// Clear the screen
				for (byte* i = (byte*)0xB8000; i < (byte*)0xB8FA0; ) {
					WriteChar((byte)' ', 0x00, i);
					i = i + 2;
				}

				byte* index = (byte*)(0xB8000);

				WriteChar((byte)'M', 0x0A, index); index = index + 2;
				WriteChar((byte)'O', 0x0A, index); index = index + 2;
				WriteChar((byte)'S', 0x0A, index); index = index + 2;
				WriteChar((byte)'A', 0x0A, index); index = index + 2;
				WriteChar((byte)' ', 0x0A, index); index = index + 2;
				WriteChar((byte)'O', 0x0A, index); index = index + 2;
				WriteChar((byte)'S', 0x0A, index); index = index + 2;
				WriteChar((byte)' ', 0x0A, index); index = index + 2;
				WriteChar((byte)'V', 0x0A, index); index = index + 2;
				WriteChar((byte)'e', 0x0A, index); index = index + 2;
				WriteChar((byte)'r', 0x0A, index); index = index + 2;
				WriteChar((byte)'s', 0x0A, index); index = index + 2;
				WriteChar((byte)'i', 0x0A, index); index = index + 2;
				WriteChar((byte)'o', 0x0A, index); index = index + 2;
				WriteChar((byte)'n', 0x0A, index); index = index + 2;
				WriteChar((byte)' ', 0x0A, index); index = index + 2;
				WriteChar((byte)'0', 0x0A, index); index = index + 2;
				WriteChar((byte)'.', 0x0A, index); index = index + 2;
				WriteChar((byte)'1', 0x0A, index); index = index + 2;
				WriteChar((byte)' ', 0x0A, index); index = index + 2;
				WriteChar((byte)'\'', 0x0A, index); index = index + 2;

				WriteChar((byte)'W', 0x0C, index); index = index + 2;
				WriteChar((byte)'a', 0x0C, index); index = index + 2;
				WriteChar((byte)'k', 0x0C, index); index = index + 2;
				WriteChar((byte)'e', 0x0C, index); index = index + 2;
				WriteChar((byte)'\'', 0x0A, index); index = index + 2;

				byte* line = (byte*)(0xB80A0);

				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;
				WriteChar((byte)'-', 0x07, line); line = line + 2;

				byte* index2 = (byte*)(0xB8140);
				WriteChar((byte)'C', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'o', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'p', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'y', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'r', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'i', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'g', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'h', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'t', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)' ', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'2', 0x0E, index2); index2 = index2 + 2;
				WriteChar((byte)'0', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'0', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'8', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'-', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'2', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'0', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'0', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)'9', 0x0A, index2); index2 = index2 + 2;
				WriteChar((byte)' ', 0x0A, index2); index2 = index2 + 2;

				while (true) {
					WriteChar((byte)'-', 0x0A, index2);
					WriteChar((byte)'\\', 0x0B, index2);
					WriteChar((byte)'|', 0x0C, index2);
					WriteChar((byte)'/', 0x0B, index2);
				}
			}
		}

		/// <summary>
		/// Writes the char.
		/// </summary>
		/// <param name="c">The c.</param>
		/// <param name="color">The color.</param>
		/// <param name="address">The address.</param>
		public unsafe static void WriteChar(byte c, byte color, byte* address)
		{
			byte* address2 = address + 1;
			*address = c;
			*address2 = color;
		}

		/// <summary>
		/// Writes the char2.
		/// </summary>
		/// <param name="a">A.</param>
		/// <param name="b">The b.</param>
		public unsafe static void Test(byte a, byte b)
		{
			byte test = (byte)(a + b);
		}


	}
}
