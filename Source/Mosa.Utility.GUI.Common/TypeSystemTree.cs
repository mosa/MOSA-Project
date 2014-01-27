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

			foreach (var assembly in typeSystem.AllAssemblies)
			{
				TreeNode moduleNode = new TreeNode(assembly.Name);
				treeView.Nodes.Add(moduleNode);

				foreach (MosaType type in typeSystem.AllTypes)
				{
					// slow!
					if (type.Assembly != assembly)
						continue;

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

					if (type.GenericBaseType != null)
					{
						TreeNode genericBaseTypeNode = new TreeNode("Generic Base Type: " + type.GenericBaseType.FullName);
						typeNode.Nodes.Add(genericBaseTypeNode);
					}

					if (type.GenericParameters.Count != 0)
					{
						TreeNode genericParameterNodes = new TreeNode("Generic Parameters");
						typeNode.Nodes.Add(genericParameterNodes);

						foreach (var genericParameter in type.GenericParameters)
						{
							TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
							genericParameterNodes.Nodes.Add(GenericParameterNode);
						}
					}

					if (type.GenericArguments.Count != 0)
					{
						TreeNode genericParameterNodes = new TreeNode("Generic Arguments");
						typeNode.Nodes.Add(genericParameterNodes);

						foreach (var genericParameter in type.GenericArguments)
						{
							TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
							genericParameterNodes.Nodes.Add(GenericParameterNode);
						}
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
							TreeNode methodNode = new ViewNode<MosaMethod>(method, method.ShortMethodName);
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

							if (method.GenericParameters.Count != 0)
							{
								TreeNode genericParameterNodes = new TreeNode("Generic Parameters");
								methodNode.Nodes.Add(genericParameterNodes);

								foreach (var genericParameter in method.GenericParameters)
								{
									TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
									genericParameterNodes.Nodes.Add(GenericParameterNode);
								}
							}

							if (method.GenericParameterTypes.Count != 0)
							{
								TreeNode genericParameterNodes = new TreeNode("Generic Parameters Types");
								methodNode.Nodes.Add(genericParameterNodes);

								foreach (var genericParameter in method.GenericParameterTypes)
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
							TreeNode methodNode = new ViewNode<MosaMethod>(method, method.ShortMethodName);
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
		}
	}
}