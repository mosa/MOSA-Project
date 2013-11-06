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

namespace Mosa.Tool.TinySimulator
{
	public partial class CallStackView : SimulatorDockContent
	{
		public CallStackView()
		{
			InitializeComponent();
		}

		private void AddSymbol(ulong ip)
		{
			SimSymbol symbol = SimCPU.FindSymbol(ip);

			string node = "[0x" + ip.ToString("X8") + "] " + (symbol != null ? symbol.Name : "Unknown");

			treeView1.Nodes.Add(node);
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
	}
}