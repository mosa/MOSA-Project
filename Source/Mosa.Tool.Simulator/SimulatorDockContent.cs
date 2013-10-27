using Mosa.TinyCPUSimulator;
using WeifenLuo.WinFormsUI.Docking;

namespace Mosa.Tool.Simulator
{
	public partial class SimulatorDockContent : DockContent
	{
		public SimulatorDockContent()
		{
			InitializeComponent();
		}

		public MainForm MainForm { get { return ((this.ParentForm) as MainForm); } }

		public SimCPU SimCPU { get { return ((this.ParentForm) as MainForm).SimCPU; } }

		public string Status { set { MainForm.Status = value; } }

		public virtual void Update()
		{
		}

		public virtual void Update(SimState simState)
		{
		}
	}
}