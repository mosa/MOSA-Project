using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dynamo;
using Mosa.Tools.TypeExplorer;

using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Tools.QuickCompilerTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private ITypeSystem typeSystem;
        private void button1_Click(object sender, EventArgs e)
        {
            var sf = new StageForm(richTextBox1.Text, "");
            sf.Show();
        }
    }
}
