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
using System.Windows.Forms;

namespace Mosa.Tool.TinySimulator
{
	public partial class ScriptView : SimulatorDockContent
	{
		private int lineNbr = 0;
		private bool wait = false;

		public ScriptView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void UpdateDock(BaseSimState simState)
		{
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			lineNbr = 0;
			wait = false;

			Execute();
		}

		public void ExecutingCompleted()
		{
			AddOutput(lineNbr, "STATUS: Execution completed");
			wait = false;
			Execute();
		}

		private void Execute()
		{
			while (!wait && richTextBox1.Lines.Length > lineNbr)
			{
				var l = richTextBox1.Lines[lineNbr];
				lineNbr++;

				string line = l.Trim();

				if (string.IsNullOrEmpty(line))
					return;

				string cmd = string.Empty;
				string data = string.Empty;

				Split(line, out  cmd, out  data);

				Execute(lineNbr, cmd, data);
			}
		}

		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			if (string.IsNullOrEmpty(openFileDialog1.FileName))
				return;

			richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			richTextBox1.Clear();
		}

		protected void Split(string line, out string first, out string rest)
		{
			int spacepos = line.IndexOf(' ');
			int tabpos = line.IndexOf('\t');

			int pos = (tabpos >= 0) ? tabpos : spacepos;

			first = string.Empty;
			rest = string.Empty;

			if (pos < 0)
			{
				first = line.Trim();
			}
			else
			{
				first = line.Substring(0, pos).Trim();
				rest = line.Substring(pos).Trim();
			}
		}

		private void toolStripLabel3_Click(object sender, EventArgs e)
		{
			int lineNbr = 0;

			foreach (var l in richTextBox1.Lines)
			{
				lineNbr++;
				string line = l.Trim();

				if (string.IsNullOrEmpty(line))
					continue;

				string cmd = string.Empty;
				string data = string.Empty;

				Split(line, out  cmd, out  data);

				Execute(lineNbr, cmd, data);
			}
		}

		protected void Execute(int lineNbr, string cmd, string data)
		{
			switch (cmd.ToLower())
			{
				case "load": LoadAssembly(lineNbr, data); return;
				case "compile": Compile(lineNbr, data); return;
				case "add-breakpoint-label": AddBreakpointByLabel(lineNbr, data); return;
				case "add-watch-label": AddWatchByLabel(lineNbr, data); return;
				case "execute": Execute(lineNbr, data); return;
				case "step": Step(lineNbr, data); return;
				case "restart": Restart(lineNbr, data); return;
				case "record": Record(lineNbr, data); return;
				default: return;
			}
		}

		protected void AddOutput(int lineNbr, string data)
		{
			MainForm.AddOutput(lineNbr.ToString() + ": " + data);
		}

		protected void LoadAssembly(int lineNbr, string data)
		{
			AddOutput(lineNbr, "STATUS: Load assembly " + data);
			MainForm.Stop();
			MainForm.LoadAssembly(data);
		}

		protected void Compile(int lineNbr, string data)
		{
			AddOutput(lineNbr, "STATUS: Compile for " + data);
			MainForm.StartSimulator(data);
		}

		protected void AddBreakpointByLabel(int lineNbr, string data)
		{
			var symbol = SimCPU.GetSymbol(data);
			AddOutput(lineNbr, "STATUS: Add breakpoint " + symbol.Name + " at " + MainForm.Format(symbol.Address, MainForm.Display32));
			MainForm.AddBreakpoint(symbol.Name, symbol.Address);
		}

		protected void Execute(int lineNbr, string data)
		{
			AddOutput(lineNbr, "STATUS: Executing!");
			wait = true;
			MainForm.Start();
		}

		protected void Restart(int lineNbr, string data)
		{
			AddOutput(lineNbr, "STATUS: Restart");
			wait = true;
			MainForm.Restart();
		}

		protected void Record(int lineNbr, string data)
		{
			if (data.Length == 0)
				Record(lineNbr, true);
			else
			{
				bool record = false;

				data = data.ToUpper();

				if (data[0] == 'Y' || data[0] == 'T' || data == "ON")
					record = true;

				Record(lineNbr, record);
			}
		}

		protected void Record(int lineNbr, bool record)
		{
			AddOutput(lineNbr, "STATUS: Record " + (record ? "On" : "Off"));
			MainForm.Record = record;
		}

		protected void Step(int lineNbr, string data)
		{
			uint step = Convert.ToUInt32(data);
			AddOutput(lineNbr, "STATUS: Executing " + step.ToString() + " steps!");
			wait = true;
			MainForm.ExecuteSteps(step);
		}

		protected void AddWatchByLabel(int lineNbr, string data)
		{
			var symbol = SimCPU.GetSymbol(data);
			AddOutput(lineNbr, "STATUS: Add watach " + symbol.Name + " at " + MainForm.Format(symbol.Address, MainForm.Display32));
			MainForm.AddWatch(symbol.Name, symbol.Address, (int)symbol.Size);
		}
	}
}