// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.DebugEngine;
using System.Collections.Generic;

namespace Mosa.Tool.Debugger
{
	public partial class DispatchOutput : DebuggerDockContent
	{
		private List<string> events;

		public DispatchOutput()
		{
			InitializeComponent();

			events = new List<string>();

			//DebugEngine.SetDispatchMethod(this, Dispatch);
		}

		public override void Connect()
		{
			Status = "Connected!";
		}

		public override void Disconnect()
		{
			Status = "Disconnected!";
		}

		private static string FormatResponseMessage(DebugMessage response)
		{
			return response.ToString();
		}

		public void ProcessResponses(DebugMessage response)
		{
			string formatted = events.Count.ToString() + ": " + FormatResponseMessage(response) + " (" + response.ID.ToString() + ")";

			events.Add(formatted);

			if (listBox1.Items.Count == 25)
			{
				listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
			}

			listBox1.Items.Insert(0, formatted);
		}
	}
}
