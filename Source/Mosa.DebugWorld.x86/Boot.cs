/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Kernel.x86;

namespace Mosa.DebugWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write('>');
			SSE.Setup();

			Test();
			Screen.Write('X');

			while (true) ;
		}

		public static bool Test()
		{
			uint address = 0x0B8050; //it's the display memory, but you can use any other adress, so far it's no critical area
			Mosa.Platform.Internal.x86.Native.Set8(address, 81); //set ascii 'Q'
			var num = Mosa.Platform.Internal.x86.Native.Get8(address); //get the 'Q' back

			#region COMPILER_BUG

			if (num >= 32 && num < 128) //COMPILER_BUG: This conditinal expression will not resolved correctly!
				return true;
			else
				return false;

			#endregion COMPILER_BUG
		}
	}
}
