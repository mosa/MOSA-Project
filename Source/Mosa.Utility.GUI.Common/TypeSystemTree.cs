/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.MosaTypeSystem;
using System.Windows.Forms;

namespace Mosa.Utility.GUI.Common
{
	public static class TypeSystemTree
	{
		public static void UpdateTree(TreeView treeView, TypeSystem typeSystem, MosaTypeLayout typeLayout, bool showSizes)
		{
			treeView.BeginUpdate();
			treeView.Nodes.Clear();

			foreach (var module in typeSystem.AllModules)
			{
				TreeNode moduleNode = new TreeNode(module.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (MosaType type in module.Types.Values)
				{
					TreeNode typeNode = new TreeNode(type.FullName);
					moduleNode.Nodes.Add(typeNode);

					if (type.BaseType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Base Type: " + type.BaseType.FullName);
						typeNode.Nodes.Add(baseTypeNode);
					}

					if (type.EnclosingType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Enclosing Type: " + type.EnclosingType.FullName);
						typeNode.Nodes.Add(baseTypeNode);
					}

					if (type.InternalType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Type Definition: " + type.InternalType.FullName);
						typeNode.Nodes.Add(baseTypeNode);
					}

					if (type.Interfaces.Count != 0)
					{
						TreeNode interfacesNodes = new TreeNode("Interfaces");
						typeNode.Nodes.Add(interfacesNodes);

						foreach (MosaType interfaceType in type.Interfaces)
						{
							TreeNode interfaceNode = new TreeNode(interfaceType.FullName);
							interfacesNodes.Nodes.Add(interfaceNode);
						}
					}

					if (type.Fields.Count != 0)
					{
						TreeNode fieldsNode = new TreeNode("Fields");
						if (showSizes)
							fieldsNode.Text = fieldsNode.Text + " (Count: " + type.Fields.Count.ToString() + " - Size: " + typeLayout.GetTypeSize(type).ToString() + ")";

						typeNode.Nodes.Add(fieldsNode);

						foreach (MosaField field in type.Fields)
						{
							TreeNode fieldNode = new TreeNode(field.DeclaringType.FullName + " " + field.Name);
							fieldsNode.Nodes.Add(fieldNode);

							if (field.IsStaticField)
								fieldNode.Text = fieldNode.Text + " [Static]";

							if (showSizes)
							{
								fieldNode.Text = fieldNode.Text + " (Size: " + typeLayout.GetFieldSize(field).ToString();

								if (!field.IsStaticField)
									fieldNode.Text = fieldNode.Text + " - Offset: " + typeLayout.GetFieldOffset(field).ToString();

								fieldNode.Text = fieldNode.Text + ")";
							}
						}
					}

					if (type.Methods.Count != 0)
					{
						TreeNode methodsNode = new TreeNode("Methods");
						typeNode.Nodes.Add(methodsNode);

						foreach (MosaMethod method in type.Methods)
						{
							TreeNode methodNode = new ViewNode<MosaMethod>(method, method.FullName);
							methodsNode.Nodes.Add(methodNode);

							if (method.IsStatic)
								methodNode.Text = methodNode.Text + " [Static]";

							if (method.IsAbstract)
								methodNode.Text = methodNode.Text + " [Abstract]";

							if (method.IsNewSlot)
								methodNode.Text = methodNode.Text + " [NewSlot]";

							if (method.IsVirtual)
								methodNode.Text = methodNode.Text + " [Virtual]";

							if (method.IsFinal)
								methodNode.Text = methodNode.Text + " [Final]";

							if (method.IsSpecialName)
								methodNode.Text = methodNode.Text + " [SpecialName]";

							if (method.IsRTSpecialName)
								methodNode.Text = methodNode.Text + " [RTSpecialName]";



							if (method.InternalMethod != null)
							{
								TreeNode baseMethodNode = new TreeNode("Method Definition: " + method.InternalMethod.FullName);
								typeNode.Nodes.Add(baseMethodNode);
							}

							if (method.GenericArguments.Count != 0)
							{
								TreeNode genericParameterNodes = new TreeNode("Generic Arguments Types");
								methodNode.Nodes.Add(genericParameterNodes);

								foreach (var genericParameter in method.GenericArguments)
								{
									TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
									genericParameterNodes.Nodes.Add(GenericParameterNode);
								}
							}
						}
					}

					if (typeLayout.GetMethodTable(type) != null)
					{
						TreeNode methodTableNode = new TreeNode("Method Table");
						typeNode.Nodes.Add(methodTableNode);

						foreach (MosaMethod method in typeLayout.GetMethodTable(type))
						{
							TreeNode methodNode = new ViewNode<MosaMethod>(method, method.FullName);
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
		}
	}
}