/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System;
using Mosa.Utility.GUI.Common;

namespace Mosa.Tool.TinySimulator
{
	public partial class AssembliesView : SimulatorDockContent
	{
		public AssembliesView()
		{
			InitializeComponent();
		}

		public void UpdateTree()
		{			
			TypeSystemTree.UpdateTree(treeView, MainForm.TypeSystem, MainForm.TypeLayout, true);
		}

		protected string FormatRuntimeMember(MosaField field)
		{
			return field.Name;
		}

		protected string FormatRuntimeMember(MosaMethod method)
		{
			return method.Name;
		}

		protected string FormatMosaType(MosaType type)
		{
			return type.Namespace + Type.Delimiter + type.Name;
		}
	}
}