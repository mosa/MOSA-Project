/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using Mosa.Compiler.TypeSystem;
using System;
using System.Collections.Generic;
using Mosa.Compiler.Linker;

namespace Mosa.Tool.Simulator
{
	public partial class SymbolView : SimulatorDockContent
	{
		public List<SymbolEntry> symbols;

		public class SymbolEntry
		{
			public LinkerSymbol LinkerSymbol;

			public string Name { get { return LinkerSymbol.Name; } }
			public long Length { get { return LinkerSymbol.Length; } }
			//public long VirtualAddress { get { return LinkerSymbol.VirtualAddress; } }
			public string VirtualAddress { get { return "0x" + LinkerSymbol.VirtualAddress.ToString("X8"); } }
			public long Offset { get { return LinkerSymbol.Offset; } }
			public SectionKind SectionKind { get { return LinkerSymbol.SectionKind; } }
			//public long SectionAddress { get { return LinkerSymbol.SectionAddress; } }
			public string SectionAddress { get { return "0x" + LinkerSymbol.SectionAddress.ToString("X8"); } }

			public SymbolEntry(LinkerSymbol linkerSymbol)
			{
				this.LinkerSymbol = linkerSymbol;
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
				symbols.Add(new SymbolEntry(symbol));
			}

			dataGridView1.DataSource = symbols;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 500;
		}

		public override void UpdateDock(SimState simState)
		{
		}

		public override void UpdateDock()
		{
		}
	}
}