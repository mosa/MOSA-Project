// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Windows.Forms;

namespace Mosa.Utility.GUI.Common
{
	public class ViewNode<T> : TreeNode
	{
		public T Type;

		public ViewNode(T type, string name)
			: base(name)
		{
			Type = type;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
