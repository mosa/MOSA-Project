// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Avalonia.Controls;
using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Tool.Explorer.Common;

namespace Mosa.Tool.Explorer.Avalonia;

public class TypeSystemTree : TypeSystemTreeBase<TreeView, TreeViewItem>
{
	public TypeSystemTree(TreeView treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizesCheck = true, HashSet<MosaUnit> includedSet = null)
		: base(treeView, typeSystem, typeLayout, showSizesCheck, includedSet)
	{
	}

	protected override void BeginUpdate()
	{
	}

	protected override void EndUpdate()
	{
	}

	protected override void ClearTree() => TreeView.Items.Clear();

	protected override TreeViewItem CreateNode(string header, object tag = null) => new TreeViewItem { Header = header, Tag = tag };

	protected override string GetHeader(TreeViewItem node) => node.Header?.ToString() ?? string.Empty;

	protected override void SetHeader(TreeViewItem node, string header) => node.Header = header;

	protected override void AddRootNode(TreeViewItem node) => TreeView.Items.Add(node);

	protected override void AddChildNode(TreeViewItem parent, TreeViewItem node) => parent.Items.Add(node);

	protected override IEnumerable<TreeViewItem> GetChildren(TreeViewItem node)
	{
		foreach (var item in node.Items)
		{
			if (item is TreeViewItem child)
				yield return child;
		}
	}
}
