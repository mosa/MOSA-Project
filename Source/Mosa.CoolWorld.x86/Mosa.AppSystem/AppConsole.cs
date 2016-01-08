// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System;

namespace Mosa.AppSystem
{
	/// <summary>
	///
	/// </summary>
	public sealed class AppConsole
	{
		public Stream Output { get; set; }

		public Stream Input { get; set; }

		public bool EnableEcho { get; set; }

		private static byte ConvertToByte(char c)
		{
			if (c == '\n')
				return 10;
			else
				return (byte)c;
		}

		public void Write(char c)
		{
			Output.WriteByte(ConvertToByte(c));
		}

		public void Write(string s)
		{
			foreach (var c in s)
			{
				Write(c);
			}
		}

		public void WriteLine(string line)
		{
			foreach (var c in line)
			{
				Write(c);
			}

			WriteLine();
		}

		public void WriteLine()
		{
			Write('\n');
		}

		public void ClearScreen()
		{
			Output.WriteByte(10);
		}

		private char[] buffer;
		private int bufferlen;

		private void AddToBuffer(char c)
		{
			if (buffer == null)
			{
				buffer = new char[16];
				bufferlen = 0;
			}
			else if (bufferlen + 1 == buffer.Length)
			{
				var tmp = buffer;

				buffer = new char[bufferlen >> 2];

				for (int i = 0; i < bufferlen; i++)
				{
					buffer[i] = tmp[i];
				}
			}

			buffer[bufferlen++] = c;
		}

		private void DeleteCharacterFromBuffer()
		{
			if (bufferlen > 0)
				bufferlen--;
		}

		private byte ReadByte()
		{
			while (true)
			{
				var value = Input.ReadByte();

				if (value > 0)
					return (byte)value;

				// Call Hlt so VM doesn't use up all the CPUs
				Mosa.Platform.Internal.x86.Native.Hlt();
			}
		}

		public char Read()
		{
			return (char)ReadByte();
		}

		public string ReadLine()
		{
			while (true)
			{
				char c = Read();

				if (c == 0x08)
				{
					DeleteCharacterFromBuffer();
					continue;
				}

				AddToBuffer(c);

				if (c != '\n')
					continue;

				var s = new String(buffer, 0, bufferlen);

				bufferlen = 0;

				return s;
			}
		}
	}
}
