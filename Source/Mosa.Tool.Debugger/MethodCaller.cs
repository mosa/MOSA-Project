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

		private bool AddData(List<int> data, ComboBox box, TextBox tb)
		{
			if (box.SelectedIndex == 0)
				return false;

			if (box.SelectedIndex == 1)
			{
				data.Add((int)tb.Text.ParseHexOrDecimal());
				return true;
			}
			else if (box.SelectedIndex == 2)
			{
				ulong val = tb.Text.ParseHexOrDecimal();
				data.Add((int)(uint)(val & 0xFFFFFFFF));
				data.Add((int)(uint)((val >> 32) & 0xFFFFFFFF));
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

			var data = new List<int>();

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

			var cmd = new List<int>();

			cmd.Add((int)address);
			cmd.Add((int)cbResultType.SelectedIndex);
			cmd.Add((int)data.Count);

			foreach (var value in data)
			{
				cmd.Add(value);
			}

			Status = "Executing...";

			SendCommand(new DebugMessage(DebugCode.ExecuteUnitTest, cmd));
		}
	}
}
