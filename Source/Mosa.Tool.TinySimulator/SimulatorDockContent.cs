using Mosa.TinyCPUSimulator;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.TinySimulator
{
	public partial class SimulatorDockContent : DockContent
	{
		public SimulatorDockContent()
		{
			InitializeComponent();
		}

		private MainForm mainForm;

		public MainForm MainForm
		{
			get
			{
				if (mainForm == null)
					mainForm = ParentForm as MainForm;

				return mainForm;
			}
		}

		public SimCPU SimCPU { get { return MainForm.SimCPU; } }

		public string Status { set { MainForm.Status = value; } }

		public virtual void UpdateDock(SimState simState)
		{
		}
	}
}