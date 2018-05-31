﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Utility.CodeCompiler;
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

			var settings = new CompilerSettings();
			settings.CodeSource = SourceCode;
			settings.AddReference("mscorlib.dll");

			var results = Utility.CodeCompiler.Compiler.ExecuteCompiler(settings);

			if (results.Errors.HasErrors)
			{
				tbErrors.Text = string.Empty;

				foreach (var error in results.Errors)
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
