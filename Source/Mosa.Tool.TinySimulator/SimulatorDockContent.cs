using Mosa.TinyCPUSimulator;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.TinySimulator
{
	public partial class SimulatorDockContent : DockContent
	{
		protected MainForm MainForm;

		public SimulatorDockContent()
		{ }

		public SimulatorDockContent(MainForm mainForm)
		{
			InitializeComponent();
			MainForm = mainForm;
		}

		public SimCPU SimCPU { get { return MainForm.SimCPU; } }

		public string Status { set { MainForm.Status = value; } }

		public virtual void UpdateDock(BaseSimState simState)
		{
		}
	}
}