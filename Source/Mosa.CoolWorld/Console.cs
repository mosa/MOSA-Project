using Mosa.ClassLib;
using Mosa.Kernel.x86;

namespace Mosa.CoolWorld.x86
{
	public class Console
	{
		private class ConsoleEntry
		{
			public byte Color;
			public byte BackgroundColor;
			public string Message;

			public ConsoleEntry(byte color, byte backgroundColor, string message)
			{
				this.Color = color;
				this.BackgroundColor = backgroundColor;
				this.Message = message;
			}
		}

		private static LinkedList<ConsoleEntry> buffer;

		public static void Initialize()
		{
			buffer = new LinkedList<ConsoleEntry>();
			buffer.Add(new ConsoleEntry(Screen.Color, Screen.BackgroundColor, string.Empty));
		}

		public static void Write(string message)
		{
			buffer.Add(new ConsoleEntry(Screen.Color, Screen.BackgroundColor, message));
			Update();
		}

		public static void WriteLine(string message)
		{
			buffer.Add(new ConsoleEntry(Screen.Color, Screen.BackgroundColor, string.Concat(message, "\n")));
			Update();
		}

		public static void WriteLine()
		{
			buffer.Add(new ConsoleEntry(Screen.Color, Screen.BackgroundColor, "\n"));
			Update();
		}

		public static void Update()
		{
			Screen.Goto(1, 0);

			var node = buffer.Last;
			var lines = CountLines();
			for (int i = 0; i < lines && node.previous != null; ++i)
				node = node.previous;

			while (node != null)
			{
				Screen.Color = node.value.Color;
				Screen.BackgroundColor = node.value.BackgroundColor;

				var s = node.value.Message;

				for (int index = 0; index < s.Length; index++)
				{
					if (s[index] == '\n')
					{
						for (uint c = 80 - Screen.Column + 1; c != 0; c--)
							Screen.Write(' ');
					}
					else
						Screen.Write(s[index]);
				}
				node = node.next;
			}
		}

		private static int CountLines()
		{
			var result = 1;
			var node = buffer.First;
			while (node != null)
			{
				var s = node.value.Message;

				for (int index = 0; index < s.Length; index++)
					++result;
				node = node.next;
			}
			return result;
		}
	}
}
