// Copyright (c) MOSA Project. Licensed under the New BSD License.

using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

// gacutil -i <assembly name>

namespace Mosa.VisualStudio.Wizard
{
	public class ProjectWizard : IWizard
	{
		void IWizard.BeforeOpeningFile(ProjectItem projectItem)
		{
		}

		void IWizard.ProjectFinishedGenerating(Project project)
		{
		}

		void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
		}

		void IWizard.RunFinished()
		{
		}

		void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
		{
			MessageBox.Show("Started!");
		}

		bool IWizard.ShouldAddProjectItem(string filePath)
		{
			return true;
		}
	}
}
