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

			AddSection(new LinkerSection(SectionKind.BSS, "BSS", 0));
			AddSection(new LinkerSection(SectionKind.Data, "Data", 0));
			AddSection(new LinkerSection(SectionKind.ROData, "ReadOnlyData", 0));
			AddSection(new LinkerSection(SectionKind.Text, "Text", 0));

			SectionAlignment = 1;
			Endianness = Endianness.Little;	// FIXME: assumes x86
		}

		#endregion Construction

		public override void Emit(System.IO.Stream stream)
		{
			//TODO!
			//	foreach (var symbol in Symbols)
			//	{
			//		SimAdapter.SimCPU.SetSymbol(symbol.Name, (ulong)symbol.VirtualAddress, (ulong)symbol.Length);
			//	}

			//	foreach (var section in LinkerSections)
			//	{
			//		ulong address = (ulong)section.VirtualAddress;

			//		var mem = (section as SimLinkerSection).Memory;

			//		foreach (byte b in mem)
			//		{
			//			if (b != 0)
			//			{
			//				SimAdapter.SimCPU.DirectWrite8(address, b);
			//			}

			//			address++;
			//		}
			//	}
			//}
		}
	}
}