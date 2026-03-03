// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Tool.Explorer.Common;

namespace Mosa.Tool.Explorer;

public class TypeSystemTree : TypeSystemTreeBase<TreeView, TreeNode>
{
	public TypeSystemTree(TreeView treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizes = true, HashSet<MosaUnit> include = null)
		: base(treeView, typeSystem, typeLayout, showSizes, include)
	{
	}

	protected override void BeginUpdate() => TreeView.BeginUpdate();

	protected override void EndUpdate() => TreeView.EndUpdate();

	protected override void ClearTree() => TreeView.Nodes.Clear();

	protected override TreeNode CreateNode(string header, object tag = null) => new TreeNode(header) { Tag = tag };

	protected override string GetHeader(TreeNode node) => node.Text;

	protected override void SetHeader(TreeNode node, string header) => node.Text = header;

	protected override void AddRootNode(TreeNode node) => TreeView.Nodes.Add(node);

	protected override void AddChildNode(TreeNode parent, TreeNode node) => parent.Nodes.Add(node);

	protected override IEnumerable<TreeNode> GetChildren(TreeNode node)
	{
		foreach (TreeNode child in node.Nodes)
			yield return child;
	}
}
