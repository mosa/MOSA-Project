
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
				WriteChar((byte)'X', 0x0B, (byte*)0xB8000);
				WriteChar((byte)'X', 0x0A, (byte*)0xB8002);

				while (true) {
					AddOne((byte*)0xB8004);
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
			*address = c;
			byte* address2 = address + 1;
			*address2 = color;
		}

		/// <summary>
		/// Adds the one.
		/// </summary>
		/// <param name="address">The address.</param>
		public unsafe static void AddOne(byte* address)
		{
			byte* address2 = address + 1;
		}

	}
}
