/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using Mosa.Utility.GUI.Common;
using System;

namespace Mosa.Tool.TinySimulator
{
	public partial class AssembliesView : SimulatorDockContent
	{
		public AssembliesView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
		}

		public void UpdateTree()
		{
			TypeSystemTree.UpdateTree(treeView, MainForm.Compiler.TypeSystem, MainForm.Compiler.TypeLayout, true);
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