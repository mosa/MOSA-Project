using Mosa.Tool.GDBDebugger.GDB;
using System;
using System.Windows.Forms;

namespace Mosa.Tool.GDBDebugger
{
    public partial class ConnectWindow : Form
    {
        public Connector Debugger
        {
            get;
            private set;
        }

        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Debugger = new Connector(new X86Platform(), tbHost.Text, (int)numPort.Value);

            DialogResult = DialogResult.OK;
        }

        private void CheckConnectButton(object sender, EventArgs e)
        {
            btnConnect.Enabled = (!string.IsNullOrEmpty(tbHost.Text));
        }
    }
}
