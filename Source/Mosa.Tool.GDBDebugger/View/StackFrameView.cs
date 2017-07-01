// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Tool.GDBDebugger.GDB;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger.View
{
	public partial class StackFrameView : DebugDockContent
	{
		public StackFrameView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void OnRunning()
		{
			listBox1.Items.Clear();
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
			listBox1.Items.Clear();

			int size = 4; // fixme

			for (int i = memory.Length - 4; i > 0; i = i - size)
			{
				ulong value = (ulong)(memory[i] | (memory[i + 1] << 8) | (memory[i + 2] << 16) | (memory[i + 3] << 24));

				var at = address + (ulong)i;
				var hexValue = BasePlatform.ToHex(value, size);
				var hexAddress = BasePlatform.ToHex(at, size);

				listBox1.Items.Add(listBox1.Items.Count.ToString("D2") + ": " + hexValue + " [" + Platform.StackFrame.Name.ToUpper() + "+" + (Platform.StackFrame.Value - at).ToString().PadLeft(2, '0') + "]");

				//listBox1.Items.Add("[" + Platform.StackFrame.Name + "+" + (Platform.StackFrame.Value - at).ToString().PadLeft(2, '0') + "] " + hexValue);
			}
		}
	}
}
