// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.CodeDomCompiler;
using System;
using System.CodeDom.Compiler;
using System.Windows.Forms;

namespace Mosa.Tool.Explorer
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

			CompilerResults results = Mosa.Utility.CodeDomCompiler.Compiler.ExecuteCompiler(settings);

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
