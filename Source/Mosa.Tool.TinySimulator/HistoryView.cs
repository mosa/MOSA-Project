/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.TinySimulator
{
	public partial class HistoryView : SimulatorDockContent
	{
		private BindingList<HistoryEntry> history = new BindingList<HistoryEntry>();

		private bool enable = true;

		private object historyLock = new object();

		private List<HistoryEntry> pendingHistory = new List<HistoryEntry>();

		private class HistoryEntry
		{
			public ulong Tick { get; private set; }

			public string IP { get; private set; }

			public ulong ip;

			public string Code { get; private set; }

			public string Method { get; private set; }

			public SimState SimState;

			public HistoryEntry(SimState simState, string method, bool force32)
			{
				this.Tick = simState.Tick;
				this.ip = simState.IP;
				this.Code = simState.Instruction.ToString();
				this.Method = method;
				this.SimState = simState;
				IP = MainForm.Format(simState.IP, force32);
			}
		}

		public HistoryView()
		{
			InitializeComponent();
			dataGridView1.DataSource = history;
		}

		public void AddHistory(SimState simState)
		{
			if (!MainForm.Record)
				return;

			string methodName = SimCPU.FindSymbol(simState.IP).Name;

			lock (pendingHistory)
			{
				pendingHistory.Add(new HistoryEntry(simState, methodName, MainForm.Display32));
			}
		}

		public override void UpdateDock(SimState simState)
		{
			if (!enable)
				return;

			if (simState == null)
				return;

			if (!MainForm.Record)
				return;

			lock (pendingHistory)
			{
				foreach (var entry in pendingHistory)
				{
					history.Add(entry);
				}

				pendingHistory.Clear();
			}

			if (history.Count == 1)
			{
				dataGridView1.AutoResizeColumns();
				dataGridView1.Columns[0].Width = 60;
				dataGridView1.Columns[1].Width = 90;
				dataGridView1.Columns[2].Width = 450;
			}

			if (history.Count > MainForm.MaxHistory + 100)
			{
				while (history.Count > MainForm.MaxHistory)
				{
					history.RemoveAt(0);
				}
			}

			dataGridView1.CurrentCell = dataGridView1[0, history.Count - 1];
			// FirstDisplayedScrollingRowIndex
			//dataGridView1.CurrentCell = dataGridView1.Rows[history.Count - 1].Cells[0];

			this.Refresh();
		}

		private void Select(SimState simState)
		{
			if (simState == null)
				return;

			enable = false;

			MainForm.UpdateAllDocks(simState);

			enable = true;
		}

		private void toolStripTextBox1_Leave(object sender, EventArgs e)
		{
			int max = 1000;

			if (Int32.TryParse(toolStripTextBox1.Text, out max))
			{
				if (max < 1)
					max = 1;

				MainForm.MaxHistory = max;
			}
		}

		private void UpdateSelect(DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0 || e.ColumnIndex < 0)
				return;

			var row = dataGridView1.Rows[e.RowIndex].DataBoundItem as HistoryEntry;

			if (row != null)
			{
				Select(row.SimState);
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			UpdateSelect(e);
		}

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			UpdateSelect(e);
		}
	}
}