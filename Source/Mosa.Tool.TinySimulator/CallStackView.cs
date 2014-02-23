/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.TinyCPUSimulator;
using System.Collections.Generic;
using System.Windows.Forms;
using System;

namespace Mosa.Tool.TinySimulator
{
	public partial class CallStackView : SimulatorDockContent
	{

		private class CallStackEntry : TreeNode
		{
			public string MethodName { get; private set; }
			public ulong Address { get; private set; }

			private SimSymbol SimSymbol;

			public CallStackEntry(string name, ulong address, bool display32)
			{
				Address = address;
				MethodName = name;
				Text = "[" + MainForm.Format(address, display32) + "] " + (string.IsNullOrEmpty(name) ? "Unknown" : name);
			}

			public CallStackEntry(ulong address, bool display32)
				: this(string.Empty, address, display32)
			{
			}

			public CallStackEntry(SimSymbol simSymbol, ulong address, bool display32)
				: this(simSymbol.Name, address, display32)
			{
				SimSymbol = simSymbol;
			}

		}

		public CallStackView()
		{
			InitializeComponent();
		}

		private void AddSymbol(ulong ip)
		{
			SimSymbol symbol = SimCPU.FindSymbol(ip);

			if (symbol != null)
			{
				treeView1.Nodes.Add(new CallStackEntry(symbol, ip, true));
			}
			else
			{
				treeView1.Nodes.Add(new CallStackEntry(ip, true));
			}
		}

		public override void UpdateDock(SimState simState)
		{
			treeView1.Nodes.Clear();

			List<ulong> list = simState.Values["CallStack"] as List<ulong>;

			foreach (ulong ip in list)
			{
				AddSymbol(ip);
			}

			this.Refresh();
		}

		private CallStackEntry clickedNode = null;

		private void treeView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			var node = treeView1.SelectedNode;

			if (node == null)
				return;

			clickedNode = node as CallStackEntry;

			var relativeMousePosition = treeView1.PointToClient(Cursor.Position);

			MenuItem menu = new MenuItem(clickedNode.Text);
			menu.Enabled = false;
			ContextMenu m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MenuItem3_Click)));
			m.MenuItems.Add(new MenuItem("&Jump to", new EventHandler(MenuItem1_Click)));
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MenuItem2_Click)));
			m.Show(treeView1, relativeMousePosition);
		}

		private void MenuItem1_Click(Object sender, EventArgs e)
		{
			if (clickedNode == null)
				return;

			SimCPU.Monitor.StepOverBreakPoint = clickedNode.Address;
		}

		private void MenuItem2_Click(Object sender, EventArgs e)
		{
			if (clickedNode == null)
				return;

			MainForm.AddBreakpoint(clickedNode.MethodName, (ulong)clickedNode.Address);
		}

		private void MenuItem3_Click(Object sender, EventArgs e)
		{
			if (clickedNode == null)
				return;

			Clipboard.SetText(clickedNode.MethodName);
		}

	}
}