/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.x86.Helpers;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class DebugUtil
	{
		private static uint Count = 0;

		public static void Trace(string location)
		{
			ConsoleSession Console = ConsoleManager.Controller.Debug;

			Console.Color = 0x0C;
			Console.BackgroundColor = 0;

			Count++;
			Console.Write("#");
			Console.Write(Count, 10, 8);
			Console.Write(" - Trace: ");
			Console.Write(location);
			Console.WriteLine();
		}

		private struct MemoryChangedItem
		{
			public uint checks;
			public ushort lastChecksum;
			public ushort bevoreLastCheckum;
		}

		private static MemoryChangedItem memoyChangedItem;

		/// <summary>
		/// Helps to track memroy changes. Checks if memroy has changed between two calls.
		/// </summary>
		/// <param name="startAddress"></param>
		/// <param name="bytes"></param>
		/// <param name="panic">Halt the kernel</param>
		/// <param name="message">Display a custom message</param>
		/// <returns></returns>
		public static bool MemoryChanged(uint startAddress, uint bytes, bool panic = false, string message = null)
		{
			//Improvement: Allocate memory for multiple items and check for different input values.
			var checksum = FlechterChecksum.Fletcher16(startAddress, bytes);
			memoyChangedItem.bevoreLastCheckum = memoyChangedItem.lastChecksum;
			memoyChangedItem.lastChecksum = checksum;
			memoyChangedItem.checks++;

			if (memoyChangedItem.checks > 1 && memoyChangedItem.bevoreLastCheckum != memoyChangedItem.lastChecksum)
			{
				if (panic)
					if (message == null)
						Panic.Error("Memory Changed Exception");
					else
						Panic.Error(message);

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
