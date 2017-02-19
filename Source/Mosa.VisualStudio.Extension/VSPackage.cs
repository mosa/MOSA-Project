// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.VisualStudio.Shell;

namespace Mosa.VisualStudio.Extension
{
	[PackageRegistration(UseManagedResourcesOnly = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
	[Guid("d8c4c7d1-b911-4130-bafb-636f2b8b0af4")]
	public class VSPackage : Package
	{
		protected override void Initialize()
		{
			base.Initialize();

			// Place additional initialization code here
		}
	}
}
