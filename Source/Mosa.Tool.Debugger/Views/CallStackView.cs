// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.Debugger.DebugData;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger.Views
{
	public partial class CallStackView : DebugDockContent
	{
		private class CallStackEntry : TreeNode
		{
			public string MethodName { get { return (Symbol == null) ? "Unknown" : Symbol.Name; } }

			public ulong InstructionPointer { get; private set; }

			public ulong StackFrame { get; private set; }

			public ulong StackPointer { get; private set; }

			private readonly SymbolInfo Symbol = null;

			public CallStackEntry(SymbolInfo symbol, ulong instructionPointer, ulong stackFrame, ulong stackPointer)
			{
				Symbol = symbol;
				InstructionPointer = instructionPointer;
				StackFrame = stackFrame;
				StackPointer = stackPointer;

				Text = "[" + ToHex(InstructionPointer) + "] " + (string.IsNullOrEmpty(MethodName) ? "Unknown" : MethodName + " [Frame: " + ToHex(StackFrame) + "]");
			}
		}

		public CallStackView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			treeView1.MouseDown += (sender, args) => treeView1.SelectedNode = treeView1.GetNodeAt(args.X, args.Y);
		}

		public override void OnRunning()
		{
			treeView1.Nodes.Clear();
		}

		protected override void ClearDisplay()
		{
			treeView1.Nodes.Clear();
		}

		protected override void UpdateDisplay()
		{
			ClearDisplay();

			if (StackFrame == 0 || InstructionPointer == 0 || StackPointer == 0)
				return;

			AddSymbol(InstructionPointer, StackFrame, StackPointer);

			var symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer);

			if (symbol != null)
			{
				// new stack frame has not been setup
				MemoryCache.ReadMemory(StackPointer, NativeIntegerSize, OnMemoryReadPrologue);
				return;
			}

			symbol = DebugSource.GetFirstSymbolsStartingAt(InstructionPointer - Platform.FirstPrologueInstructionSize);

			if (symbol != null)
			{
				// new stack frame has not been setup
				MemoryCache.ReadMemory(StackPointer + NativeIntegerSize, NativeIntegerSize, OnMemoryReadPrologue);
				return;
			}

			MemoryCache.ReadMemory(StackFrame, NativeIntegerSize * 2, OnMemoryRead);
		}

		private void AddSymbol(ulong ip, ulong stackFrame, ulong stackPointer)
		{
			var symbol = DebugSource.GetFirstSymbol(ip);

			treeView1.Nodes.Add(new CallStackEntry(symbol, ip, stackFrame, stackPointer));
		}

		private void OnMemoryRead(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplay(address, bytes)));

		private void OnMemoryReadPrologue(ulong address, byte[] bytes) => Invoke((MethodInvoker)(() => UpdateDisplayPrologue(address, bytes)));

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			if (memory.Length < NativeIntegerSize * 2)
				return; // something went wrong!

			if (treeView1.Nodes.Count == 0)
				return; // race condition

			var returnStackFrame = MainForm.ToLong(memory, 0, NativeIntegerSize);
			var returnIP = MainForm.ToLong(memory, NativeIntegerSize, NativeIntegerSize);

			if (returnIP == 0)
				return;

			AddSymbol(returnIP, returnStackFrame, address);

			if (returnStackFrame != 0)
			{
				if (treeView1.Nodes.Count > 20)
					return;

				MemoryCache.ReadMemory(returnStackFrame, NativeIntegerSize * 2, OnMemoryRead);
			}
		}

		private void UpdateDisplayPrologue(ulong address, byte[] memory)
		{
			if (memory.Length < NativeIntegerSize)
				return; // something went wrong!

			if (treeView1.Nodes.Count == 0)
				return; // race condition

			ulong returnIP = MainForm.ToLong(memory, 0, NativeIntegerSize);

			if (returnIP == 0)
				return;

			AddSymbol(returnIP, StackFrame, address);

			MemoryCache.ReadMemory(StackFrame, NativeIntegerSize * 2, OnMemoryRead);
		}

		private void treeView1_MouseClick(object sender, MouseEventArgs e)
		{
			var node = treeView1.SelectedNode;

			if (node == null)
				return;

			var clickedEntry = node as CallStackEntry;

			if (e.Button == MouseButtons.Left)
			{
				MainForm.SetFocus(clickedEntry.InstructionPointer, clickedEntry.StackFrame, clickedEntry.StackPointer);
			}
			else if (e.Button == MouseButtons.Right)
			{
				var relativeMousePosition = treeView1.PointToClient(Cursor.Position);

				var menu = new ToolStripMenuItem(clickedEntry.Text);
				menu.Enabled = false;

				var m = new ContextMenuStrip();
				m.Items.Add(menu);
				m.Items.Add(new ToolStripMenuItem("Copy to &Clipboard", null, new EventHandler(MainForm.OnCopyToClipboard)) { Tag = ToHex(clickedEntry.InstructionPointer) });
				m.Items.Add(new ToolStripMenuItem("Set &Breakpoint", null, new EventHandler(MainForm.OnAddBreakPoint)) { Tag = new AddBreakPointArgs(null, clickedEntry.InstructionPointer) });
				m.Show(treeView1, relativeMousePosition);
			}
		}
	}
}
