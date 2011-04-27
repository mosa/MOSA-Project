using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mosa.Runtime;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Tools.CompilerHelper;
using Mosa.Tools.StageVisualizer;
using System.CodeDom.Compiler;
using System.IO;

namespace Mosa.Tools.TypeExplorer
{
	public partial class StageForm : Form
	{
		string methodname;
		private ITypeSystem typeSystem;
		StringLogger sl = new StringLogger();
		Output output;
		public CompilerHelperSettings settings = new CompilerHelperSettings();
		public StageForm(ITypeSystem typeSystem, string methodname)
		{
			this.methodname = methodname;
			//Mosa.Tools.CompilerHelper.CompilerHelper.FilterMethod(methodname);
			Mosa.Tools.CompilerHelper.CompilerHelper.SetLogger(sl);
			this.typeSystem = typeSystem;
			InitializeComponent();
			RunMosaCompiler();
			output = new Output(sl.GetFullLog());
			tbSource.Lines = output.Lines;
			UpdateText(null, null);
			int methodindex = cbMethods.FindString(methodname);
			cbMethods.SelectedIndex = methodindex;
			cbMethods_SelectionChangeCommitted(null, null);
		}

		public StageForm(string sourcecode, string methodname)
		{
			this.methodname = methodname;
			InitializeComponent();
			settings.CodeSource = sourcecode;
			settings.AddReference("mscorlib.dll"); 
			settings.AddReference("Mosa.Kernel.dll");
			string assembly = RunCodeDomCompiler(settings);
			Mosa.Tools.CompilerHelper.CompilerHelper.SetLogger(sl);
			RunMosaCompiler(settings, assembly);
			output = new Output(sl.GetFullLog());
			tbSource.Lines = output.Lines;
			UpdateText(null, null);
			int methodindex = cbMethods.FindString(methodname);
			cbMethods.SelectedIndex = methodindex;
			cbMethods_SelectionChangeCommitted(null, null);
		}

		/// <summary>
		/// A cache of CodeDom providers.
		/// </summary>
		private static Dictionary<string, CodeDomProvider> providerCache = new Dictionary<string, CodeDomProvider>();
		/// <summary>
		/// 
		/// </summary>
		private static string tempDirectory;
		/// <summary>
		/// Holds the temporary files collection.
		/// </summary>
		private static TempFileCollection temps = new TempFileCollection(TempDirectory, false);

		private static string TempDirectory
		{
			get
			{
				if (tempDirectory == null)
				{
					tempDirectory = Path.Combine(Path.GetTempPath(), "mosa");
					if (!Directory.Exists(tempDirectory))
					{
						Directory.CreateDirectory(tempDirectory);
					}
				}
				return tempDirectory;
			}
		}

		private string RunCodeDomCompiler(CompilerHelperSettings settings)
		{
			Console.WriteLine("Executing {0} compiler...", settings.Language);

			CodeDomProvider provider;
			if (!providerCache.TryGetValue(settings.Language, out provider))
			{
				provider = CodeDomProvider.CreateProvider(settings.Language);
				if (provider == null)
					throw new NotSupportedException("The language '" + settings.Language + "' is not supported on this machine.");
				providerCache.Add(settings.Language, provider);
			}

			string filename = Path.Combine(TempDirectory, Path.ChangeExtension(Path.GetRandomFileName(), "dll"));
			temps.AddFile(filename, false);

			string[] references = new string[settings.References.Count];
			settings.References.CopyTo(references, 0);

			CompilerResults compileResults;
			CompilerParameters parameters = new CompilerParameters(references, filename, false);
			parameters.CompilerOptions = "/optimize-";

			if (settings.UnsafeCode)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /unsafe+";
				else
					throw new NotSupportedException();
			}

			if (settings.DoNotReferenceMscorlib)
			{
				if (settings.Language == "C#")
					parameters.CompilerOptions = parameters.CompilerOptions + " /nostdlib";
				else
					throw new NotSupportedException();
			}

			parameters.GenerateInMemory = false;

			if (settings.CodeSource != null)
			{
				//Console.WriteLine("Code: {0}", settings.CodeSource + settings.AdditionalSource);
				compileResults = provider.CompileAssemblyFromSource(parameters, settings.CodeSource + settings.AdditionalSource);
			}
			else
				throw new NotSupportedException();

			if (compileResults.Errors.HasErrors)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Code compile errors:");
				foreach (CompilerError error in compileResults.Errors)
				{
					sb.AppendLine(error.ToString());
				}
				throw new Exception(sb.ToString());
			}

			return compileResults.PathToAssembly;
		}

		private void RunMosaCompiler(CompilerHelperSettings settings, string assemblyFile)
		{
			IAssemblyLoader assemblyLoader = new AssemblyLoader();
			assemblyLoader.InitializePrivatePaths(settings.References);

			assemblyLoader.LoadModule(assemblyFile);

			foreach (string file in settings.References)
			{
				assemblyLoader.LoadModule(file);
			}

			typeSystem = new TypeSystem();
			typeSystem.LoadModules(assemblyLoader.Modules);
			RunMosaCompiler();
		}

		private void RunMosaCompiler()
		{
			Mosa.Tools.CompilerHelper.CompilerHelper.Compile(typeSystem);
			//TestAssemblyLinker linker = TestCaseAssemblyCompiler.Compile(typeSystem);

		   // return linker;
		}

		private void UpdateText(object sender, EventArgs e)
		{
			cbMethods.Items.Clear();

			foreach (string item in output.GetMethods())
				cbMethods.Items.Add(item);

			cbMethods.SelectedIndex = 0;
			cbMethods_SelectionChangeCommitted(sender, e);
		}

		private void cbMethods_SelectionChangeCommitted(object sender, EventArgs e)
		{
			if (output != null)
			{
				cbStages.Items.Clear();

				foreach (string item in output.GetStages(cbMethods.SelectedItem.ToString()))
					cbStages.Items.Add(item);

				cbStages.SelectedIndex = 0;

				cbStages_SelectionChangeCommitted(sender, e);
			}
		}
		private void cbStages_SelectionChangeCommitted(object sender, EventArgs e)
		{
			cbStage.Checked = true;

			string method = cbMethods.SelectedItem.ToString();
			string stage = string.Empty;

			if (cbStages.SelectedItem != null)
				stage = cbStages.SelectedItem.ToString();

			string label = string.Empty;

			if (cbLabels.SelectedItem != null)
				label = cbLabels.SelectedItem.ToString();

			cbLabels.Items.Clear();

			foreach (string item in output.GetLabels(method, stage))
				cbLabels.Items.Add(item);

			if (!string.IsNullOrEmpty(label))
				if (cbLabels.Items.Contains(label))
					cbLabels.SelectedItem = label;

			refreshButton_Click(sender, e);
		}

		private void cbLabels_SelectionChangeCommitted(object sender, EventArgs e)
		{
			cbLabel.Checked = true;
			refreshButton_Click(sender, e);
		}

		private void tbSource_TextChanged(object sender, EventArgs e)
		{
			output = new Output(tbSource.Lines);
			UpdateText(sender, e);
			lbStatus.Text = DateTime.Now.ToString();
		}
		private void refreshButton_Click(object sender, EventArgs e)
		{
			if (cbMethods.SelectedItem == null)
			{
				tbResult.Lines = new string[0];
				return;
			}

			string method = cbMethods.SelectedItem.ToString();
			string stage = string.Empty;

			if (cbStages.SelectedItem != null)
				stage = cbStages.SelectedItem.ToString();

			string label = string.Empty;

			if (cbLabels.SelectedItem != null)
				label = cbLabels.SelectedItem.ToString();

			if (!cbLabel.Checked)
				label = string.Empty;

			if (!cbStage.Checked)
				stage = string.Empty;

			List<string> lines = output.GetText(method, stage, label, removeNextprevInformationToolStripMenuItem.Checked, spaceAfterBlockToolStripMenuItem.Checked);

			string[] final = new string[lines.Count];

			for (int i = 0; i < lines.Count; i++)
				final[i] = lines[i];

			tbResult.Lines = final;
		}

	}
}
