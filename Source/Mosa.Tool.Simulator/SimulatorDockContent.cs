using Mosa.TinyCPUSimulator.Adaptor;
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

		public ISimAdapter SimAdapter { get { return ((this.ParentForm) as MainForm).SimAdapter; } }

		public string Status { set { MainForm.Status = value; } }

		public virtual void Update()
		{
		}
	}
}