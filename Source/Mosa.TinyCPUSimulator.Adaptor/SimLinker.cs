/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	/// </summary>
	public class SimLinker : BaseLinker
	{
		/// <summary>
		/// The cpu
		/// </summary>
		public ISimAdapter SimAdapter { get; protected set; }

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SimLinker" /> class.
		/// </summary>
		/// <param name="simAdapter">The sim adapter.</param>
		public SimLinker(ISimAdapter simAdapter)
		{
			this.SimAdapter = simAdapter;

			AddSection(new SimLinkerSection(SectionKind.BSS, "BSS", 0x400000, 0x200000, simAdapter));
			AddSection(new SimLinkerSection(SectionKind.Data, "Data", 0x600000, 0x200000, simAdapter));
			AddSection(new SimLinkerSection(SectionKind.ROData, "ReadOnlyData", 0x800000, 0x200000, simAdapter));
			AddSection(new SimLinkerSection(SectionKind.Text, "Text", 0xA00000, 0x200000, simAdapter));

			LoadSectionAlignment = 1;
			SectionAlignment = 1;
			Endianness = Endianness.Little;	// FIXME: assumes x86
		}

		protected override void CreateFile()
		{
			base.CreateFile();

			foreach (var symbol in Symbols)
			{
				SimAdapter.SimCPU.SetSymbol(symbol.Name, (ulong)symbol.VirtualAddress, (ulong)symbol.Length);
			}

			foreach (var section in Sections)
			{
				ulong address = (ulong)section.VirtualAddress;

				var mem = (section as SimLinkerSection).Memory;

				foreach (byte b in mem)
				{
					if (b != 0)
					{
						SimAdapter.SimCPU.DirectWrite8(address, b);
					}

					address++;
				}
			}
		}

		#endregion Construction
	}
}