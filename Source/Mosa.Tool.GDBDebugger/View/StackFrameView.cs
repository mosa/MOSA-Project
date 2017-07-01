// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class StackFrameView : DebugDockContent
	{
		private class StackEntry
		{
			public string Offset { get; set; }

			public string Address { get; set; }

			public string Value { get; set; }

			[Browsable(false)]
			public int Index { get; set; }
		}

		public StackFrameView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnRunning()
		{
			dataGridView1.DataSource = null;
		}

		public override void OnPause()
		{
			if (Platform == null)
				return;

			if (Platform.Registers == null)
				return;

			var span = Platform.StackFrame.Value - Platform.StackPointer.Value;

			if (span <= 0)
				span = 4;
			else if (span > 512)
				span = 512;

			if (span % 4 != 0)
				span = span + (span % 4);

			GDBConnector.ReadMemory(Platform.StackFrame.Value - span, (int)span + 4, OnMemoryRead);
		}

		private void OnMemoryRead(ulong address, byte[] bytes)
		{
			MethodInvoker method = delegate ()
			{
				UpdateDisplay(address, bytes);
			};

			BeginInvoke(method);
		}

		private void UpdateDisplay(ulong address, byte[] memory)
		{
			var list = new List<StackEntry>();

			int size = 4; // fixme

			for (int i = memory.Length - 4; i > 0; i = i - size)
			{
				ulong value = (ulong)(memory[i] | (memory[i + 1] << 8) | (memory[i + 2] << 16) | (memory[i + 3] << 24));

				var at = address + (ulong)i;

				var entry = new StackEntry()
				{
					Address = BasePlatform.ToHex(at, size),
					Value = BasePlatform.ToHex(value, size),
					Offset = Platform.StackFrame.Name.ToUpper() + "-" + (Platform.StackFrame.Value - at).ToString().PadLeft(2, '0'),
					Index = list.Count,
				};

				list.Add(entry);
			}

			dataGridView1.DataSource = list;
			dataGridView1.AutoResizeColumns();
		}
	}
}
