using System;

namespace Mosa.HelloWorld
{
	public static class Boot
	{
		public static void Main()
		{
			// Start the boot process

			// Write HelloWorld to Screen
			unsafe {
				byte* index = (byte*)(0xB8000);

				*(index++) = (byte)'H';
				*(index++) = 1;
				*(index++) = (byte)'E';
				*(index++) = 1;
				*(index++) = (byte)'L';
				*(index++) = 1;
				*(index++) = (byte)'L';
				*(index++) = 1;
				*(index++) = (byte)'O';
				*(index++) = 1;
				*(index++) = (byte)' ';
				*(index++) = 1;
				*(index++) = (byte)'W';
				*(index++) = 1;
				*(index++) = (byte)'O';
				*(index++) = 1;
				*(index++) = (byte)'R';
				*(index++) = 1;
				*(index++) = (byte)'L';
				*(index++) = 1;
				*(index++) = (byte)'D';
				*(index++) = 1;
				*(index++) = (byte)'!';
				*(index++) = 1;
			}

		}
	}
}
