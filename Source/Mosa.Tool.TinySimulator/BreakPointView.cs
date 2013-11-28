/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.TinySimulator
{
	public partial class BreakPointView : SimulatorDockContent
	{
		private BindingList<BreakPointEntry> breakpoints = new BindingList<BreakPointEntry>();

		private class BreakPointEntry
		{
			public string Name { get; private set; }

			public string Hex { get; private set; }

			public ulong Address { get; private set; }

			private bool display32;

			public BreakPointEntry(ulong address, bool display32)
			{
				this.Address = address;
				this.Hex = MainForm.Format(address, display32);
				this.display32 = display32;
			}

			public BreakPointEntry(string name, ulong address, bool display32)
				: this(address, display32)
			{
				this.Name = name;
			}
		}

		public BreakPointView()
		{
			InitializeComponent();
			dataGridView1.DataSource = breakpoints;
			dataGridView1.AutoResizeColumns();
			dataGridView1.Columns[0].Width = 250;
		}

		public void AddBreakPoint(string name, ulong address)
		{
			var breakpoint = new BreakPointEntry(name, address, true);

			breakpoints.Add(breakpoint);

			SimCPU.Monitor.AddBreakPoint(address);
		}

		private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete)
				return;

			if (dataGridView1.CurrentCell == null)
				return;

			var row = dataGridView1.CurrentCell.OwningRow.DataBoundItem;

			breakpoints.Remove(row as BreakPointEntry);

			SimCPU.Monitor.RemoveBreakPoint((row as BreakPointEntry).Address);
		}
	}
}