// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

[ComVisible(true)]
public class VSProjectNode : ProjectNode
{
	private VSPackage package;

	public VSProjectNode(VSPackage package)
	: base()
	{
		this.package = package;
	}

	public override Guid ProjectGuid
	{
		get { return typeof(MyProjectFactory).GUID; }
	}

	public override string ProjectType
	{
		get { return "VSProject"; }
	}

	public override void AddFileFromTemplate(string source, string target)
	{
		//this.FileTemplateProcessor.UntokenFile(source, target);
		//this.FileTemplateProcessor.Reset();
	}
}
