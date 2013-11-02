/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Linker;
using Mosa.TinyCPUSimulator;
using System.Collections.Generic;

namespace Mosa.Tool.Simulator
{
	public partial class SymbolView : SimulatorDockContent
	{
		private List<SymbolEntry> symbols;

		private class SymbolEntry
		{
			public LinkerSymbol LinkerSymbol;
			private bool force32;

			public string Name { get { return LinkerSymbol.Name; } }

			public long Length { get { return LinkerSymbol.Length; } }

			//public long VirtualAddress { get { return LinkerSymbol.VirtualAddress; } }
			public string VirtualAddress { get { return MainForm.Format(LinkerSymbol.VirtualAddress, force32); } }

			public long Offset { get { return LinkerSymbol.Offset; } }

			public SectionKind SectionKind { get { return LinkerSymbol.SectionKind; } }

			//public long SectionAddress { get { return LinkerSymbol.SectionAddress; } }
			public string SectionAddress { get { return MainForm.Format(LinkerSymbol.SectionAddress, force32); } }

			public SymbolEntry(LinkerSymbol linkerSymbol, bool force32)
			{
				this.LinkerSymbol = linkerSymbol;
				this.force32 = force32;
			}
		}

		public SymbolView()
		{
			InitializeComponent();
		}

		public void CreateEntries()
		{
			symbols = new List<SymbolEntry>(MainForm.Linker.Symbols.Count);

			foreach (var symbol in MainForm.Linker.Symbols)
			{
				symbols.Add(new SymbolEntry(symbol, true)); // true == forces 32 bit
			}

			dataGridView1.DataSource = symbols;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 500;
		}

		public override void UpdateDock(SimState simState)
		{
		}

	}
}