// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Utility.DebugEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mosa.Tool.Debugger
{
	/// <summary>
	///
	/// </summary>
	public partial class MethodCaller : DebuggerDockContent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCaller"/> class.
		/// </summary>
		public MethodCaller()
		{
			InitializeComponent();
			cbParameter1.SelectedIndex = 0;
			cbParameter2.SelectedIndex = 0;
			cbParameter3.SelectedIndex = 0;
			cbResultType.SelectedIndex = 0;
		}

		public override void Connect()
		{
		}

		public override void Disconnect()
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//SetUnitTestMethodAddress();
			ExecuteUnitTest();
		}

		private bool AddData(List<uint> data, ComboBox box, TextBox tb)
		{
			if (box.SelectedIndex == 0)
				return false;

			if (box.SelectedIndex == 1)
			{
				data.Add((uint)tb.Text.ParseHexOrDecimal());
				return true;
			}
			else if (box.SelectedIndex == 2)
			{
				ulong val = tb.Text.ParseHexOrDecimal();
				data.Add((uint)(uint)(val & 0xFFFFFFFF));
				data.Add((uint)(uint)((val >> 32) & 0xFFFFFFFF));
				return true;
			}
			else
			{
				throw new Exception();
			}
		}

		private void ExecuteUnitTest()
		{
			uint address = 0;

			try
			{
				address = (uint)tbMethodAddress.Text.ParseHexOrDecimal();
			}
			catch { }

			if (address == 0)
			{
				Status = "ERROR: Invalid address parameter";
				return;
			}

			var data = new List<uint>();

			try
			{
				if (AddData(data, cbParameter1, tbParameter1))
					if (AddData(data, cbParameter2, tbParameter2))
						AddData(data, cbParameter3, tbParameter3);
			}
			catch
			{
				Status = "Invalid Parameter";
				return;
			}

			var cmd = new uint[4 + 4 + 4 + data.Count];

			cmd[0] = address;
			cmd[1] = (uint)cbResultType.SelectedIndex;
			cmd[2] = (uint)data.Count;

			uint index = 3;
			foreach (var value in data)
			{
				cmd[index] = value;
				index++;
			}

			Status = "Executing...";

			SendCommand(new DebugMessage(DebugCode.ExecuteUnitTest, cmd));
		}
	}
}
