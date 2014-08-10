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

			foreach (var module in typeSystem.Modules)
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

					if (type.DeclaringType != null)
					{
						TreeNode baseTypeNode = new TreeNode("Enclosing Type: " + type.DeclaringType.FullName);
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
							TreeNode fieldNode = new TreeNode(field.ShortName);
							fieldsNode.Nodes.Add(fieldNode);

							if (field.IsStatic)
								fieldNode.Text = fieldNode.Text + " [Static]";

							if (showSizes)
							{
								fieldNode.Text = fieldNode.Text + " (Size: " + typeLayout.GetFieldSize(field).ToString();

								if (!field.IsStatic)
									fieldNode.Text = fieldNode.Text + " - Offset: " + typeLayout.GetFieldOffset(field).ToString();

								fieldNode.Text = fieldNode.Text + ")";
							}
						}
					}

					if (type.Properties.Count != 0)
					{
						TreeNode propertiesNode = new TreeNode("Properties");
						if (showSizes)
							propertiesNode.Text = propertiesNode.Text + " (Count: " + type.Properties.Count.ToString() + ")";

						typeNode.Nodes.Add(propertiesNode);

						foreach (MosaProperty property in type.Properties)
						{
							TreeNode propertyNode = new ViewNode<MosaProperty>(property, property.ShortName);
							propertiesNode.Nodes.Add(propertyNode);

							if (property.GetterMethod != null)
							{
								TreeNode getterNode = new ViewNode<MosaMethod>(property.GetterMethod, property.GetterMethod.ShortName);
								propertyNode.Nodes.Add(getterNode);

								if (property.GetterMethod.IsStatic)
									getterNode.Text = getterNode.Text + " [Static]";

								if (property.GetterMethod.IsAbstract)
									getterNode.Text = getterNode.Text + " [Abstract]";

								if (property.GetterMethod.IsNewSlot)
									getterNode.Text = getterNode.Text + " [NewSlot]";

								if (property.GetterMethod.IsVirtual)
									getterNode.Text = getterNode.Text + " [Virtual]";

								if (property.GetterMethod.IsFinal)
									getterNode.Text = getterNode.Text + " [Final]";

								if (property.GetterMethod.IsSpecialName)
									getterNode.Text = getterNode.Text + " [SpecialName]";

								if (property.GetterMethod.IsRTSpecialName)
									getterNode.Text = getterNode.Text + " [RTSpecialName]";

								if (property.GetterMethod.GenericArguments.Count != 0)
								{
									TreeNode genericParameterNodes = new TreeNode("Generic Arguments Types");
									getterNode.Nodes.Add(genericParameterNodes);

									foreach (var genericParameter in property.GetterMethod.GenericArguments)
									{
										TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
										genericParameterNodes.Nodes.Add(GenericParameterNode);
									}
								}
							}

							if (property.SetterMethod != null)
							{
								TreeNode setterNode = new ViewNode<MosaMethod>(property.SetterMethod, property.SetterMethod.ShortName);
								propertyNode.Nodes.Add(setterNode);

								if (property.SetterMethod.IsStatic)
									setterNode.Text = setterNode.Text + " [Static]";

								if (property.SetterMethod.IsAbstract)
									setterNode.Text = setterNode.Text + " [Abstract]";

								if (property.SetterMethod.IsNewSlot)
									setterNode.Text = setterNode.Text + " [NewSlot]";

								if (property.SetterMethod.IsVirtual)
									setterNode.Text = setterNode.Text + " [Virtual]";

								if (property.SetterMethod.IsFinal)
									setterNode.Text = setterNode.Text + " [Final]";

								if (property.SetterMethod.IsSpecialName)
									setterNode.Text = setterNode.Text + " [SpecialName]";

								if (property.SetterMethod.IsRTSpecialName)
									setterNode.Text = setterNode.Text + " [RTSpecialName]";

								if (property.SetterMethod.GenericArguments.Count != 0)
								{
									TreeNode genericParameterNodes = new TreeNode("Generic Arguments Types");
									setterNode.Nodes.Add(genericParameterNodes);

									foreach (var genericParameter in property.SetterMethod.GenericArguments)
									{
										TreeNode GenericParameterNode = new TreeNode(genericParameter.Name);
										genericParameterNodes.Nodes.Add(GenericParameterNode);
									}
								}
							}
						}
					}

					if (type.Methods.Count != 0)
					{
						TreeNode methodsNode = new TreeNode("Methods");
						typeNode.Nodes.Add(methodsNode);

						foreach (MosaMethod method in type.Methods)
						{
							if (method.ShortName.StartsWith("set_") || method.ShortName.StartsWith("get_")) continue;

							TreeNode methodNode = new ViewNode<MosaMethod>(method, method.ShortName);
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
							TreeNode methodNode = new ViewNode<MosaMethod>(method, method.ShortName);
							methodTableNode.Nodes.Add(methodNode);
						}
					}
				}
			}

			treeView.EndUpdate();
		}
	}
}