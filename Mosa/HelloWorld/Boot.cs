
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
				byte* index = (byte*)(0xB8000);

				for (byte* i = (byte*)0xB8000; i < (byte*)0xB8FA0; ) {
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
				WriteChar((byte)0x0C, index++);
				WriteChar((byte)'a', index++);
				WriteChar((byte)0x0C, index++);
				WriteChar((byte)'k', index++);
				WriteChar((byte)0x0C, index++);
				WriteChar((byte)'e', index++);
				WriteChar((byte)0x0C, index++);
				WriteChar((byte)'\'', index++);
				WriteChar((byte)0x0A, index++);
				WriteChar((byte)' ', index++);
				WriteChar((byte)0x0A, index++);
				WriteChar((byte)' ', index++);
				WriteChar((byte)0x0A, index++);
				WriteChar((byte)' ', index++);

				byte* line = (byte*)(0xB80A0);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);
				WriteChar((byte)0x07, line++);
				WriteChar((byte)'-', line++);

				byte* index_2 = (byte*)(0xB8140);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'C', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'o', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'p', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'y', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'r', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'i', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'g', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'h', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'t', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)' ', index_2++);
				WriteChar((byte)0x0E, index_2++);
				WriteChar((byte)'2', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'0', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'0', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'8', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'-', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'2', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'0', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'0', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)'9', index_2++);
				WriteChar((byte)0x0A, index_2++);
				WriteChar((byte)' ', index_2++);
				while (true) {
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

		/// <summary>
		/// Tests this instance.
		/// </summary>
		public static void Test(int x, int y)
		{
			int i = (x + y);
			int z = i + 1;
			int q = z * 2;
		}

		/// <summary>
		/// Tests the specified x.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="z">The z.</param>
		public static void Test2(int x, int y, int z)
		{
		}
	}
}
