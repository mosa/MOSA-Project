using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mosa.TinyCPUSimulator;
using Mosa.TinyCPUSimulator.Adaptor;

namespace Mosa.Tool.Explorer
{
	public partial class SimProcessorX86Form : Form
	{
		protected ISimAdapter simAdapter;

		public SimProcessorX86Form(ISimAdapter simAdapter)
		{
			InitializeComponent();

			this.simAdapter = simAdapter;
			this.simAdapter.Monitor.EnableStepping = true;
			GetCurrentStateAndUpdate();
		}

		private void SimProcessorX86Form_Load(object sender, EventArgs e)
		{

		}

		public void Update(SimState simState)
		{
			lbCurrentFrame.Items.Clear();
			lbFlags.Items.Clear();
			lbGPRs.Items.Clear();
			lbXMMRegisters.Items.Clear();
			lbStack.Items.Clear();

			lbGPRs.Items.Add("EIP: " + simState.Values["EIP"]);
			lbGPRs.Items.Add("EAX: " + simState.Values["EAX"]);
			lbGPRs.Items.Add("EBX: " + simState.Values["EBX"]);
			lbGPRs.Items.Add("ECX: " + simState.Values["ECX"]);
			lbGPRs.Items.Add("EDX: " + simState.Values["EDX"]);
			lbGPRs.Items.Add("ESP: " + simState.Values["ESP"]);
			lbGPRs.Items.Add("EBP: " + simState.Values["EBP"]);
			lbGPRs.Items.Add("ESI: " + simState.Values["ESI"]);
			lbGPRs.Items.Add("EDI: " + simState.Values["EDI"]);

			//simState.StoreValue("XXM0", XMM0.Value.ToString());
			//simState.StoreValue("XXM1", XMM1.Value.ToString());
			//simState.StoreValue("XXM2", XMM2.Value.ToString());
			//simState.StoreValue("XXM3", XMM3.Value.ToString());
			//simState.StoreValue("XXM4", XMM4.Value.ToString());
			//simState.StoreValue("XXM5", XMM5.Value.ToString());
			//simState.StoreValue("XXM6", XMM6.Value.ToString());
			//simState.StoreValue("XXM7", XMM7.Value.ToString());

			//lbXMMRegisters;

			lbFlags.Items.Add("Zero: " + simState.Values["FLAGS.Zero"]);
			lbFlags.Items.Add("Carry: " + simState.Values["FLAGS.Carry"]);
			lbFlags.Items.Add("Sign: " + simState.Values["FLAGS.Sign"]);
			lbFlags.Items.Add("Overflow: " + simState.Values["FLAGS.Overflow"]);
			lbFlags.Items.Add("Parity: " + simState.Values["FLAGS.Parity"]);
			lbFlags.Items.Add("Direction: " + simState.Values["FLAGS.Direction"]);
			lbFlags.Items.Add("Adjust: " + simState.Values["FLAGS.Adjust"]);
			lbFlags.Items.Add("FLAGS: " + simState.Values["FLAGS"]);

			int count = Convert.ToInt32(simState.Values["StackFrame.Index.Count"]);

			for (int index = 0; index < count; index++)
			{
				lbCurrentFrame.Items.Add(index.ToString("D2") + ": " + simState.Values["StackFrame.Index." + index.ToString()]);
			}

			count = Convert.ToInt32(simState.Values["Stack.Index.Count"]);

			for (int index = count - 1; index >= 0; index--)
			{
				lbStack.Items.Add(index.ToString("D2") + ": " + simState.Values["Stack.Index." + index.ToString()]);
			}

			lbStack.SelectedIndex = lbStack.Items.Count - 1;

		}

		public void GetCurrentStateAndUpdate()
		{
			var state = simAdapter.GetState();

			lbInstructionHistory.Items.Add(state);
			lbInstructionHistory.SelectedIndex = lbInstructionHistory.Items.Count - 1;
		}

		public void ExecuteSteps(int steps)
		{
			try
			{
				for (int i = 0; i < steps; i++)
				{
					simAdapter.Execute();
					if (cbRecord.Checked)
					{
						GetCurrentStateAndUpdate();
					}
				}
			}
			catch (CPUException e)
			{
				GetCurrentStateAndUpdate();
				lbInstructionHistory.Items.Add(e);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			ExecuteSteps(1);
		}

		private void lbInstructionHistory_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (lbInstructionHistory.SelectedItem is SimState)
				Update(lbInstructionHistory.SelectedItem as SimState);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			ExecuteSteps((int)nvSteps.Value);
		}
	}
}
