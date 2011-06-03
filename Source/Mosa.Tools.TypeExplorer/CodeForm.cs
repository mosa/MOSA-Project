using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.CodeDom.Compiler;

using Mosa.Test.CodeDomCompiler;

namespace Mosa.Tools.TypeExplorer
{
	public partial class CodeForm : Form
	{
		public CodeForm()
		{
			InitializeComponent();
		}

		public string SourceCode { get { return tbText.Text; } }
		public string Assembly { get; set; }

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.None;

			CompilerSettings settings = new CompilerSettings();
			settings.CodeSource = SourceCode;
			settings.AddReference("mscorlib.dll");

			CompilerResults results = Mosa.Test.CodeDomCompiler.Compiler.ExecuteCompiler(settings);

			if (results.Errors.HasErrors)
			{
				tbErrors.Text = string.Empty;

				foreach (CompilerError error in results.Errors)
				{
					tbErrors.AppendText(error.ToString());
					tbErrors.AppendText("\n");
				}
			}
			else
			{
				Assembly = results.PathToAssembly;
				DialogResult = DialogResult.OK;
				Close();
			}
		}


	}
}
