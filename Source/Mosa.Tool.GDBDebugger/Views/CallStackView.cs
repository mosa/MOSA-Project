// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.DebugData;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.Views
{
	public partial class CallStackView : DebugDockContent
	{
		private class CallStackEntry : TreeNode
		{
			public string MethodName { get { return (Symbol == null) ? "Unknown" : Symbol.Name; } }

			public string HexAddress { get { return Address.ToString((Address <= uint.MaxValue) ? "X4" : "X8"); } }

			public ulong Address { get; private set; }

			private readonly SymbolInfo Symbol = null;

			public CallStackEntry(SymbolInfo symbol, ulong address)
			{
				Symbol = symbol;
				Address = address;

				Text = "[0x" + HexAddress + "] " + (string.IsNullOrEmpty(MethodName) ? "Unknown" : MethodName);
			}
		}

		public CallStackView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnRunning()
		{
			treeView1.Nodes.Clear();
		}

		public override void OnPause()
		{
			treeView1.Nodes.Clear();

			ulong ebp = Platform.StackFrame.Value;
			ulong ip = Platform.InstructionPointer.Value;

			if (ebp == 0 || ip == 0)
				return;

			AddSymbol(ip);

			if (ebp != 0)
			{
				MemoryCache.ReadMemory(ebp, 8, OnMemoryRead);
			}
		}

		private void AddSymbol(ulong ip)
		{
			var symbol = DebugSource.GetFirstSymbol(ip);

			treeView1.Nodes.Add(new CallStackEntry(symbol, ip));
		}

		private void OnMemoryRead(ulong address, byte[] bytes)
		{
			MethodInvoker method = delegate ()
			{
				UpdateDisplay(address, bytes);
			};

			BeginInvoke(method);
		}

		public static ulong ToLong(byte[] bytes, int start, int size) // future: make this common
		{
			ulong value = 0;

			for (int i = 0; i < size; i++)
			{
				ulong shifted = (ulong)(bytes[start + i] << (i * 8));
				value |= shifted;
			}

			return value;
		}

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			if (memory.Length < 8)
				return; // something went wrong!

			if (treeView1.Nodes.Count == 0)
				return; // race condition

			ulong ebp = ToLong(memory, 0, 4);
			ulong ip = ToLong(memory, 4, 4);

			if (ip == 0)
				return;

			AddSymbol(ip);

			if (ebp != 0)
			{
				if (treeView1.Nodes.Count > 20)
					return;

				MemoryCache.ReadMemory(ebp, 8, OnMemoryRead);
			}
		}

		private void treeView1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
				return;

			var node = treeView1.SelectedNode;

			if (node == null)
				return;

			var clickedEntry = node as CallStackEntry;

			var relativeMousePosition = treeView1.PointToClient(Cursor.Position);

			var menu = new MenuItem(clickedEntry.Text);
			menu.Enabled = false;
			var m = new ContextMenu();
			m.MenuItems.Add(menu);
			m.MenuItems.Add(new MenuItem("Copy to &Clipboard", new EventHandler(MainForm.OnCopyToClipboard)) { Tag = clickedEntry.HexAddress });
			m.MenuItems.Add(new MenuItem("Set &Breakpoint", new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.Address) });
			m.Show(treeView1, relativeMousePosition);
		}
	}
}
