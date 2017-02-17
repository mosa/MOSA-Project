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
using Microsoft.VisualStudio.Shell.Flavor;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using IServiceProvider = System.IServiceProvider;

namespace Mosa.VisualStudio.VSIXProject
{
	[Guid(VSProjectFactory.VSProjectFactoryGuidString)]
	public class VSProjectFactory : FlavoredProjectFactoryBase
	{
		public const string VSProjectFactoryGuidString = "719def2a-b4e6-4acc-82f3-4218ac9b44a9";

		protected VSPackage package;

		public VSProjectFactory(VSPackage package)
			: base()
		{
			this.package = package;
		}

		#region IVsAggregatableProjectFactory

		protected override object PreCreateForOuter(IntPtr outerProjectIUnknown)
		{
			//CustomPropertyPageProjectFlavor newProject = new CustomPropertyPageProjectFlavor();
			//newProject.Package = this.package;
			//return newProject;

			VSProjectNode project = new VSProjectNode(package);

			project.SetSite((IOleServiceProvider)((IServiceProvider)package).GetService(typeof(IOleServiceProvider)));
			return project;
		}

		#endregion IVsAggregatableProjectFactory
	}
}
