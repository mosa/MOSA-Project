/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata;
using Mosa.Compiler.MosaTypeSystem;
using System;
using Mosa.Utility.GUI.Common;

namespace Mosa.Tool.TinySimulator
{
	public partial class AssembliesView : SimulatorDockContent
	{
		private bool showTokenValues = true;

		public AssembliesView()
		{
			InitializeComponent();
		}

		public void UpdateTree()
		{			
			TypeSystemTree.UpdateTree(treeView, MainForm.TypeSystem, MainForm.TypeLayout, true);
		}

		protected string TokenToString(Token token)
		{
			return token.ToInt32().ToString("X8");
		}

		protected string FormatToString(Token token)
		{
			if (!showTokenValues)
				return string.Empty;

			return "[" + TokenToString(token) + "] ";
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