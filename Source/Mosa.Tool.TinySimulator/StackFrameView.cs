// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.TinyCPUSimulator;
using System.Collections.Generic;

namespace Mosa.Tool.TinySimulator
{
	public partial class StackFrameView : SimulatorDockContent
	{
		public StackFrameView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public override void UpdateDock(BaseSimState simState)
		{
			listBox1.Items.Clear();

			bool display32 = simState.NativeRegisterSize == 32;
			List<ulong[]> stack = simState.Values["StackFrame"] as List<ulong[]>;

			foreach (var entry in stack)
			{
				listBox1.Items.Add(listBox1.Items.Count.ToString("D2") + ": " + MainForm.Format(entry[0], display32) + " [" + MainForm.Format(entry[1], display32) + "]");
			}

			Refresh();
		}
	}
}
