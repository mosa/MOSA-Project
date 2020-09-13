// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Reko.Core;
using Reko.Core.Machine;
using System.Collections.Generic;
using System.IO;

namespace Mosa.Utility.Disassembler
{
	public partial class Disassembler
	{
		public class InstrWriter : MachineInstructionWriter
		{
			private readonly TextWriter writer;
			private int chars;
			private readonly List<string> annotations;

			public InstrWriter(TextWriter writer)
			{
				this.writer = writer;
				this.annotations = new List<string>();
				this.Platform = null;
			}

			public IPlatform Platform { get; }

			public Address Address { get; set; }

			public void Tab()
			{
				++chars;
				writer.Write("\t");
			}

			public void WriteString(string s)
			{
				chars += s.Length;
				writer.Write(s);
			}

			public void WriteUInt32(uint n)
			{
				var nn = n.ToString();
				chars += nn.Length;
				writer.Write(nn);
			}

			public void WriteChar(char c)
			{
				++chars;
				writer.Write(c);
			}

			public void WriteFormat(string fmt, params object[] parms)
			{
				var s = string.Format(fmt, parms);
				chars += s.Length;
				writer.Write(s);
			}

			public void WriteAddress(string formattedAddress, ulong uAddr)
			{
				chars += formattedAddress.Length;
				writer.Write(formattedAddress);
			}

			public void WriteAddress(string formattedAddress, Address addr)
			{
				chars += formattedAddress.Length;
				writer.Write(formattedAddress);
			}

			public void WriteMnemonic(string sMnemonic)
			{
				chars += sMnemonic.Length;
				writer.Write(sMnemonic);
			}

			public void WriteLine()
			{
				if (annotations.Count > 0)
				{
					var pad = 60 - chars;
					if (pad > 0)
					{
						writer.Write(new string(' ', pad));
						chars += pad;
					}
					WriteString("; ");
					WriteString(string.Join(", ", annotations));
					annotations.Clear();
				}
				chars = 0;
				writer.WriteLine();
			}

			public void AddAnnotation(string annotation)
			{
				annotations.Add(annotation);
			}
		}
	}
}
